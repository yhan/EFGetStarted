namespace Tests;

public class Tests
{
    [Test]
    public void Test1()
    {
        var dates = new List<string>()
        {
            "2019-01-15",
            "2000-12-16",
        };
        var ordered = dates.OrderBy(x => x).ToArray();
        Check.That(ordered[0]).IsEqualTo("2000-12-16");
        Check.That(ordered[1]).IsEqualTo("2019-01-15");
    }

    [Test]
    public void EfCoreHealth()
    {
        using var dbContext = new OrderContext();
        foreach (var blog in dbContext.Blogs)
        {
            TestContext.WriteLine(blog);
        }
    }

    [Test]
    public void EfCoreHealth_WithDbContextFactory()
    {
        using var dbContext = new OrderContext();
        foreach (var blog in dbContext.Blogs)
        {
            TestContext.WriteLine(blog);
        }
    }
}