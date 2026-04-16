using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExploreAzureCloud.Data;
using System.ComponentModel.DataAnnotations;

namespace ExploreAzureCloud.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly ExploreAzureCloud.Data.ApplicationDbContext _context;

        public CreateModel(ExploreAzureCloud.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptName");
            return Page();
        }

        [BindProperty]
        public EmployeeDto Employee { get; set; } = default!;
        
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var EmployeeEntity = new Employee()
            {
                Name=Employee.Name,
                Phone=Employee.Phone,
                DepartmentId=Employee.DepartmentId
            };
            _context.Employees.Add(EmployeeEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

public class EmployeeDto
{
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(12)]
    public required string Phone{ get; set; }
    public required Guid DepartmentId { get; set; }
}