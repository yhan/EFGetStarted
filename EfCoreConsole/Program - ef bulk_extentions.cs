//// See https://aka.ms/new-console-template for more information

//using System.ComponentModel.DataAnnotations.Schema;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using EFCore.BulkExtensions;
//using Microsoft.EntityFrameworkCore;

//Debug.WriteLine("Hello, World!");
//Random rand = new Random();
//string[] types = new[] { "SOR", "CLIENT" };
//int batchSize = 4_000;

//HashSet<string> _orderIds = new HashSet<string>();

//var alwaysOnDbContext = new OrderContext();

//using (var db = new OrderContext())
//    // Remove all
//    db.Database.ExecuteSqlRaw(@"truncate table hello.""Orders""");

//using (var db = new OrderContext())
//    _orderIds = db.Orders.Select(x => x.Id).ToHashSet();



//var newOrders = new List<Order>();
//var updateOrders = new Dictionary<string, Order>();

//while (true)
//{
//    var watch = Stopwatch.StartNew();
//    for (int i = 0; i < batchSize; i++)
//    {
//        var msg = RandomOder(rand.Next(0, batchSize));
//        if (!_orderIds.Contains(msg.Id))
//        {
//            _orderIds.Add(msg.Id);
//            newOrders.Add(msg);
//        }
//        else
//        {
//            updateOrders[msg.Id] = msg; // order already with random value
//        }
//    }

//    Debug.WriteLine($"[bench] figure out new and update: {watch.ElapsedTicks} ticks");
   
//    // round strip
//    watch.Restart();

//    alwaysOnDbContext.BulkInsert(newOrders);
//    alwaysOnDbContext.BulkUpdate(updateOrders.Values.ToList()/*, operation => operation.ColumnPrimaryKeyExpression = o => o.Id*/);

//    Debug.WriteLine($"[bench] Inserted '{newOrders.Count}' Updated '{updateOrders.Count}': {watch.ElapsedMilliseconds}"); // below 100 ms

//    newOrders.Clear();
//}


//Order RandomOder(int i)
//{
//    return new Order
//    {
//        Id = i.ToString(),
//        Type = types[rand.Next(0, 2)],
//        VWAP = rand.NextDouble() * 100
//    };
//}