// See https://aka.ms/new-console-template for more information

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

using (var db = new OrderContext())
{
    // Create
    Console.WriteLine("Inserting a new Order");
    db.Add(new Order
    {
        Id = "helloOrder",
        ParentOrder = null,
        Type = "SOR",
        VWAP = 42D
    });
    db.SaveChanges();

    // Read
    Console.WriteLine("Querying for a blog");
    var order = db.Orders
        .OrderBy(b => b.Id)
        .First();

    // Update
    Console.WriteLine("Create a parent order. And assign the create order to an existing order");
    var parent = new Order
    {
        Id = "parentOrder",
        ParentOrder = null,
        Type = "Parent",
        VWAP = 42D*023
    };
    order.Type = "ClientOrder";
    db.Add(parent);
    db.SaveChanges();

    // Delete
    Console.WriteLine("Delete the order");
    db.Remove(order);
    db.SaveChanges();
}

public class Order
{
    public string Id { get; set; }
    public string Type { get; set; }

    public double VWAP { get; set; }

    public string? ParentOrderId { get; set; }

    //[ForeignKey("ParentOrderId")]
    public Order ParentOrder { get; set; }
    public ICollection<Order> Children { get; set; }

}

public class OrderContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(
            "Server=127.0.0.1;Port=5432;Database=dbEfCore;User Id=postgres;Password=postgres;CommandTimeout=20;SearchPath=hello;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(order =>
        {
            order.HasMany(e => e.Children)
                .WithOne(e => e.ParentOrder)
                .HasForeignKey(e => e.ParentOrderId)
                .IsRequired(false); // allows FK to be null
        });

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
