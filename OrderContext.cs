using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public class OrderContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<Blog> Blogs { get; set; }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var conn = new NpgsqlConnection(
            "Server=127.0.0.1;Port=5432;Database=dbEfCore;User Id=postgres;Password=postgres;CommandTimeout=20;SearchPath=hello;");
        conn.StateChange += (sender, e) =>
        {
            Debug.WriteLine($"CNX origin ={e.OriginalState} new={e.CurrentState}");
        };

        options.UseNpgsql(conn)
            .LogTo(StdOut)
            .EnableSensitiveDataLogging();
    }

    private void StdOut(string obj)
    {
        Debug.WriteLine(obj);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(order =>
        {
            order.HasMany(e => e.Children)
                .WithOne(e => e.ParentOrder)
                .HasForeignKey(e => e.ParentOrderId)
                .IsRequired(false); // allows FK to be null
        });

        modelBuilder.HasPostgresEnum<Mood>();
        modelBuilder.Entity<Blog>(g => g.HasKey(g => g.Id));

        /* Created table
         *
         *-- Table: hello.Orders
           
           -- DROP TABLE IF EXISTS hello."Orders";
           
           CREATE TABLE IF NOT EXISTS hello."Orders"
           (
           "Id" text COLLATE pg_catalog."default" NOT NULL,
           "Type" text COLLATE pg_catalog."default" NOT NULL,
           "VWAP" double precision NOT NULL,
           "ParentOrderId" text COLLATE pg_catalog."default" NOT NULL,
           CONSTRAINT "PK_Orders" PRIMARY KEY ("Id"),
           CONSTRAINT "FK_Orders_Orders_ParentOrderId" FOREIGN KEY ("ParentOrderId")
           REFERENCES hello."Orders" ("Id") MATCH SIMPLE
           ON UPDATE NO ACTION
           ON DELETE CASCADE
           )
           WITH (
           OIDS = FALSE
           )
           TABLESPACE pg_default;
           
           ALTER TABLE IF EXISTS hello."Orders"
           OWNER to postgres;
           -- Index: IX_Orders_ParentOrderId
           
           -- DROP INDEX IF EXISTS hello."IX_Orders_ParentOrderId";
           
           CREATE INDEX IF NOT EXISTS "IX_Orders_ParentOrderId"
           ON hello."Orders" USING btree
           ("ParentOrderId" COLLATE pg_catalog."default" ASC NULLS LAST)
           TABLESPACE pg_default;
         *
         */
    }
}

public class Blog
{
    public int Id { get; set; }
    public Mood Mood { get; set; }
}

public enum Mood { Happy, Sad }