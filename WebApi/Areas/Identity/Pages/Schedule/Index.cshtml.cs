using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.Schedule
{
    public class IndexModel : PageModel
    {
        private readonly MainDbContext _context;

        public IndexModel(MainDbContext context)
        {
            _context = context;
        }

        public List<Recipe> TuesdayRecipes;
        public async Task<IActionResult> OnGet()
        {
            TuesdayRecipes = await _context.Recipe.ToListAsync();
            return Page();
        }
    }
}
