// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

Debug.WriteLine("Hello, World!");
Random rand = new Random();
string[] types = new[] { "SOR", "CLIENT" };
int batchSize = 4_000;

HashSet<string> _orderIds = new HashSet<string>();

var alwaysOnDbContext = new OrderContext();

using (var db = new OrderContext())
    // Remove all
    db.Database.ExecuteSqlRaw(@"truncate table hello.""Orders""");

using (var db = new OrderContext())
    _orderIds = db.Orders.Select(x => x.Id).ToHashSet();



var newOrders = new Dictionary<string, Order>();
var updateOrders = new Dictionary<string, Order>();

int round = 0;
while (true)
{
    round++;
    var watch = Stopwatch.StartNew();
    
    for (int i = 0; i < batchSize; i++)
    {
        var msg = RandomOder(rand.Next(0, batchSize));

        if (!_orderIds.Contains(msg.Id))
        {
            _orderIds.Add(msg.Id);
            newOrders.Add(msg.Id, msg);
        }
        else
        {
            newOrders.Remove(msg.Id);

            updateOrders[msg.Id] = msg; // order already with random value
        }
    }

    if (newOrders.Values.Select(o => o.Id).Distinct().Count() > newOrders.Count)
    {
        
    }

    IEnumerable<string> enumerable = updateOrders.Values.Select(o => o.Id);
    IEnumerable<string> comparer = newOrders.Values.Select(o => o.Id);
    var intersection = enumerable.Intersect(comparer,
        EqualityComparer<string>.Default);

    if (intersection.Any())
    {

    }

    Debug.WriteLine($"[bench] figure out new and update: {watch.ElapsedTicks} ticks");

    var orders = newOrders.Values.ToList();
    orders.AddRange(updateOrders.Values);

    // round strip
    watch.Restart();
    
    alwaysOnDbContext.BulkInsertOrUpdate(orders);
    
    Debug.WriteLine($"[bench] Upserted '{orders.Count}' : {watch.ElapsedMilliseconds}"); // below 100 ms

    newOrders.Clear();
    updateOrders.Clear();
    
}


Order RandomOder(int i)
{
    return new Order
    {
        Id = i.ToString(),
        Type = types[rand.Next(0, 2)],
        VWAP = rand.NextDouble() * 100
    };
}