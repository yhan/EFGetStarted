using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class EmployeeContext: ConfigureDbContext
{
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeSalary>(g => g.HasKey(g => g.EmpNo));
    }
}

public class EmployeeContextFactory : IDesignTimeDbContextFactory<EmployeeContext>
{
    public EmployeeContext CreateDbContext(string[] args)
    {
        var connString = args[0];

        var optionsBuilder = new DbContextOptionsBuilder<EmployeeContext>();
        optionsBuilder.UseNpgsql(connString);

        return new EmployeeContext(optionsBuilder.Options);
    }
}

public class EmployeeSalary
{
    public string DepName { get; set; }
    public long EmpNo { get; set; }
    public int Salary { get; set; }
}
