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

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public List<string> DayMenuName { get; set; } = new List<string>();
            public List<List<Recipe>> Recipes = new List<List<Recipe>>();
        }

        public IndexModel(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList).ThenInclude(x => x.Recipe).ToListAsync();
            foreach (var dayMenu in dayMenus)
            {
                List<Recipe> recipes = new List<Recipe>();
                foreach (var recipeList in dayMenu.RecipeList)
                    recipes.Add(recipeList.Recipe);
                Input.Recipes.Add(recipes);
                Input.DayMenuName.Add(dayMenu.Name);
            }
            return Page();
        }
    }
}
