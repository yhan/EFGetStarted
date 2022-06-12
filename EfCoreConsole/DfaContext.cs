using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFGetStarted;

public class DfaContext : ConfigureDbContext
{
    public DbSet<DfaDetail> DfaDetails { get; set; }
    
    public DfaContext(DbContextOptions<DfaContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DfaDetail>(g => g.HasKey(d => d.Id));
    }
}

public class DfaContextFactory : IDesignTimeDbContextFactory<DfaContext>
{
    public DfaContext CreateDbContext(string[] args)
    {
        var connString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<DfaContext>();
        optionsBuilder.UseNpgsql(connString);

        return new DfaContext(optionsBuilder.Options);
    }
}

public class DfaDetail
{
    public long Id { get; set; }
    public string OrderId { get; set; }
    public OrderLevel OrderLevel { get; set; }
    public DateTimeOffset EventTime { get; set; }
    public EventType EventType { get; set; }
    public int ExecutedQty { get; set; }
    public int TargetQty { get; set; }

    public override string ToString()
    {
        return $"{Id}: [{OrderId}] {OrderLevel} @{EventTime} state={EventType} {ExecutedQty}/{TargetQty}";
    }
}

public enum EventType
{
    New,
    PartiallyExecuted,
    Executed,
    Cancelled,
    Rejected
}

public enum OrderLevel
{
    Parent,
    Child
}
