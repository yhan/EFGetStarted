using EFGetStarted;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("hello");


await using var context = new OrderContext();
context.Blogs.Add(new Blog { Mood = Mood.Happy});
await context.SaveChangesAsync();

var blogs = await context.Blogs.ToListAsync();
foreach (var b in blogs)
{
    Console.WriteLine($"blog: {b.Id} mood={b.Mood}");
}


// --auto - generated definition
//     create type mood as
//
// enum ('happy', 'sad');
//
// alter type mood owner to postgres;
