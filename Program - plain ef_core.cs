//// See https://aka.ms/new-console-template for more information

//using System.ComponentModel.DataAnnotations.Schema;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
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

//var prodConso = new Thread(() =>
//{
//    var toBeUpdated = new HashSet<string>();

//    while (true)
//    {
//        var watch = Stopwatch.StartNew();
//        for (int i = 0; i < batchSize; i++)
//        {
//            var msg = RandomOder(rand.Next(0, batchSize));
//            if (!_orderIds.Contains(msg.Id))
//            {
//                _orderIds.Add(msg.Id);
//                alwaysOnDbContext.Add(msg);
//            }
//            else
//            {
//                toBeUpdated.Add(msg.Id);
//            }
//        }

//        Debug.WriteLine($"[bench] figure out new and update: {watch.ElapsedMilliseconds}");
//        watch.Restart();

//        // cnx open
//        var toBeUpdateOrders = alwaysOnDbContext.Orders.Where(x => toBeUpdated.Contains(x.Id));
//        // cnx close
//        Debug.WriteLine($"[bench] Select to be updated: {watch.ElapsedTicks}");


//        foreach (var ord in toBeUpdateOrders)
//        {
//            // apply random update
//            ord.VWAP *= rand.NextDouble();
//        }

//        // round strip
//        watch.Restart();
//        alwaysOnDbContext.SaveChanges();
//        Debug.WriteLine($"[bench] Save all: {watch.ElapsedMilliseconds}");
//    }
//});
//prodConso.Name = "prod_conso";
//prodConso.Start();



//Order RandomOder(int i)
//{
//    return new Order
//    {
//        Id = i.ToString(),
//        Type = types[rand.Next(0, 2)],
//        VWAP = rand.NextDouble() * 100
//    };
//}