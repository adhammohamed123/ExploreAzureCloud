using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExploreAzureCloud.Data;

namespace ExploreAzureCloud.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly ExploreAzureCloud.Data.ApplicationDbContext _context;

        public IndexModel(ExploreAzureCloud.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Department = await _context.Departments.ToListAsync();
        }
    }
}
