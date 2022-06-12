using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public abstract class ConfigureDbContext : DbContext
{
    protected ConfigureDbContext()
    {
    }
    protected ConfigureDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var conn = new NpgsqlConnection(
        "Server=127.0.0.1;Port=5432;Database=dbEfCore;User Id=postgres;Password=postgres;CommandTimeout=20;SearchPath=hello;");
        conn.StateChange += (sender, e) => {
            Debug.WriteLine($"CNX origin ={e.OriginalState} new={e.CurrentState}");
        };

        options.UseNpgsql(conn)
            .LogTo(s => Debug.WriteLine(s))
            .EnableSensitiveDataLogging();
    }
}
