using Microsoft.EntityFrameworkCore;
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
        public  Department Department { get; set; }
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
}
