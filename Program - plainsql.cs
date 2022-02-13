//// See https://aka.ms/new-console-template for more information

//using System.ComponentModel.DataAnnotations.Schema;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Text;
//using Microsoft.EntityFrameworkCore;
//using Npgsql;

//Debug.WriteLine("Hello, World!");
//Random rand = new Random();
//string[] types = new[] { "SOR", "CLIENT" };
//int batchSize = 4_000;

//HashSet<string> _orderIds = new HashSet<string>();


//using (var db = new OrderContext())
//    // Remove all
//    db.Database.ExecuteSqlRaw(@"truncate table hello.""Orders""");

//using (var db = new OrderContext())
//    _orderIds = db.Orders.Select(x => x.Id).ToHashSet();

//var query = new StringBuilder();

//var connection =
//    new NpgsqlConnection(
//        "Server=127.0.0.1;Port=5432;Database=dbEfCore;User Id=postgres;Password=postgres;CommandTimeout=20;SearchPath=hello;");
//connection.Open();

//var toBeUpdated = new HashSet<string>();
//while (true)
//{
//    var watch = Stopwatch.StartNew();
//    for (int i = 0; i < batchSize; i++)
//    {
//        var msg = RandomOder(rand.Next(0, batchSize));
//        if (!_orderIds.Contains(msg.Id))
//        {
//            _orderIds.Add(msg.Id);
//            query.Append(
//                $"INSERT INTO hello.\"Orders\"(\r\n\t\"Id\", \"Type\", \"VWAP\", \"ParentOrderId\")\r\n\tVALUES ('{msg.Id}', '{msg.Type}', {msg.VWAP}, NULL); {Environment.NewLine}");
//        }
//        else
//        {
//            toBeUpdated.Add(msg.Id);

//            // apply random update
//            msg.VWAP *= rand.NextDouble();

//            query.Append(@$"UPDATE hello.""Orders""
//	SET ""Type""='{msg.Type}', ""VWAP""={msg.VWAP}
//	WHERE ""Id""='{msg.Id}'; {Environment.NewLine}");
//        }
//    }

//    Debug.WriteLine($"[bench] figure out new and update: {watch.ElapsedMilliseconds}");

//    watch.Restart();

//    int affected = 0;
//    using var cmd = new NpgsqlCommand(query.ToString(), connection);
//        affected = cmd.ExecuteNonQuery();

//    Debug.WriteLine($"[bench] Save {affected} orders: {watch.ElapsedMilliseconds}");
//    query.Clear();
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