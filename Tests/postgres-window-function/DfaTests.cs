using EFCore.BulkExtensions;
using EFGetStarted;
using Microsoft.EntityFrameworkCore;

namespace Tests;

[TestFixture]
public class DfaTests
{
    private static string[] ParentIds;
    private static readonly Random random = new();
    private static EventType[] eventTypes;
    [OneTimeSetUp]
    public void Init()
    {
        ParentIds = new string[26];
        for (int i = 0; i < 26; i++)
        {
            ParentIds[i] = ((char)('a' + i)).ToString();
        }
        eventTypes = Enum.GetValues<EventType>().ToArray();
    }
    
    [Test]
    public void Ingest()
    {
        var parentDetails = new List<DfaDetail>();
        var memory = new Dictionary<string, DateTimeOffset>();
        for (int i = 0; i < 100; )
        {
            var parentId = ParentIds[random.Next(0, 26)];
            if(!memory.ContainsKey(parentId))
            {
                var newEventTime = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(random.Next(60 * 60)));
                var firstDetail = new DfaDetail
                {
                    Id = i++,
                    OrderId = parentId,
                    EventTime = newEventTime,
                    EventType = EventType.New,
                    OrderLevel = OrderLevel.Parent
                };
                parentDetails.Add(firstDetail);
                memory.Add(parentId, newEventTime);
            }
            const int excludeNew = 1;
            var detail = new DfaDetail
            {
                Id = i++,
                OrderId = parentId,
                EventTime = memory[parentId].Add(TimeSpan.FromSeconds(random.Next(60 * 60))),
                EventType = eventTypes[random.Next(excludeNew, eventTypes.Length)],
                OrderLevel = OrderLevel.Parent
            };
            parentDetails.Add(detail);
        }
        using var context = new DfaContextFactory().CreateDbContext(new[] { Consts.ConnString });
        context.BulkInsertOrUpdate(parentDetails);
    }

    [Test]
    public void LiveParents()
    {
        using var context = new DfaContextFactory().CreateDbContext(new[] { Consts.ConnString });
        {
            var lastEventsQuery = from d in context.DfaDetails
                where d.OrderLevel == OrderLevel.Parent
                group d by d.OrderId into g
                select g.OrderBy(d => d.EventTime).Last(); // sql query 

            var lastEvents = lastEventsQuery.ToList(); // in memory
            var liveParents = from evt in lastEvents
                where evt.EventType == EventType.New || evt.EventType == EventType.PartiallyExecuted
                select evt;

            foreach (var lp in liveParents.ToList())
            {
                TestContext.WriteLine(lp);
            }
        }
    }

    [Test]
    public void LiveParents_FailedToTranslateToSql()
    {
        using var context = new DfaContextFactory().CreateDbContext(new[] { Consts.ConnString });
        {
            var liveParents = from evt in 
            (from d in context.DfaDetails
                where d.OrderLevel == OrderLevel.Parent
                group d by d.OrderId
                into g
                select g.OrderBy(d => d.EventTime).Last())
                where evt.EventType== EventType.PartiallyExecuted || evt.EventType== EventType.New
                    select evt;
            
            foreach (var lp in liveParents.ToList())
            {
                TestContext.WriteLine(lp);
            }
        }
    }

    [Test]
    public void LiveParentRawSql()
    {
        using var context = new DfaContextFactory().CreateDbContext(new[] { Consts.ConnString });
        {
            var query = @"select * from (
                          SELECT ""Id"", ""EventTime"", ""EventType"", ""ExecutedQty"", ""OrderId"", ""OrderLevel"", ""TargetQty"",
                                 rank() OVER(PARTITION BY ""OrderId"" ORDER BY ""EventTime"" DESC) AS rank
                                       FROM ""DfaDetails""
                                       WHERE ""OrderLevel"" = 0
                         ) ss
                         where ss.rank=1 and ss.""EventType"" in (0, 1);";
                                    
            var liveParents = context.DfaDetails.FromSqlRaw(query);

            foreach (var lp in liveParents.ToList())
            {
                TestContext.WriteLine(lp);
            }
        }
    }
}
