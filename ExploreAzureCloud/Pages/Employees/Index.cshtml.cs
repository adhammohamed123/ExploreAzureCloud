using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExploreAzureCloud.Data;

namespace ExploreAzureCloud.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly ExploreAzureCloud.Data.ApplicationDbContext _context;

        public IndexModel(ExploreAzureCloud.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Employee = await _context.Employees
                .Include(e => e.Department).ToListAsync();
        }
    }
}
