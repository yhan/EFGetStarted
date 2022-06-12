using EFCore.BulkExtensions;

namespace Tests;

/// <summary>
/// postgres window function
/// https://www.postgresql.org/docs/current/tutorial-window.html
/// </summary>
[TestFixture]
public class EmployeeTests
{
    private static readonly Random random = new();
    private static readonly string[] Departments = {
        "develop", "personnel", "sales"
    };
    private const string ConnString = "Server=127.0.0.1;Port=5432;Database=dbEfCore;User Id=postgres;Password=postgres;CommandTimeout=20;SearchPath=hello;";
    
    [Test]
    public void Ingest()
    {
        var salaries = new List<EmployeeSalary>();
        for (int i = 0; i < 100; i++)
        {
            var salary = new EmployeeSalary
            {
                EmpNo = i,
                Salary = random.Next(1000, 5001),
                DepName = Departments[random.Next(0, Departments.Length)]
            };
            salaries.Add(salary);
        }

        using var context = new EmployeeContextFactory().CreateDbContext(new[] { ConnString });
        context.BulkInsertOrUpdate(salaries);
    }
}
