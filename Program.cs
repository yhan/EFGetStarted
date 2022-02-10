// See https://aka.ms/new-console-template for more information

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

    public Order ParentOrder { get; set; }
}

public class OrderContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public OrderContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=dbEfCore;User Id=postgres;Password=postgres;CommandTimeout=20;SearchPath=hello;");
}
