using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreAzureCloud.Data
{
    public class Employee
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public required string  Name { get; set; }

        [MaxLength(12)]
        public required string  Phone { get; set; }

        [ForeignKey(nameof(Department))] // this is convention already
        public required Guid DepartmentId { get; set; }
        
        public virtual Department Department { get; set; }
    }


    public class Department
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public required string DeptName { get; set; }

        [MaxLength(500)]
        public required string  Location { get; set; }

        public ICollection<Employee> Employees { get; set; } = [];

    }


    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        
    }


    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {



        public ApplicationDbContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: true)
                                 .AddJsonFile("appsettings.Development.json", optional: true)
                                 .AddUserSecrets<ApplicationDbContextFactory>()
                                 .AddEnvironmentVariables()
                                 .Build();

            var connectionStr = configuration.GetConnectionString("AzureSqlConStr"); // we add it to local user secrets

            //if (connectionStr == null)
                //throw new ArgumentNullException(" no connection string found are you sure it is added in secrets");

            var contextoptions = new DbContextOptionsBuilder<ApplicationDbContext>();

            contextoptions.UseAzureSql(connectionStr, options => options.EnableRetryOnFailure());
            
            return new ApplicationDbContext(contextoptions.Options);
        }
    }
}
