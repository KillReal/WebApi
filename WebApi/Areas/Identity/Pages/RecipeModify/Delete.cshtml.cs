using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.RecipeModify
{
    public class DeleteModel : PageModel
    {
        private readonly MainDbContext _context;

        public DeleteModel(MainDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGet(long id)
        {
            Recipe = await _context.Recipe.Include(x => x.RecipeList)
                                        .FirstOrDefaultAsync(x => x.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            var recipe = await _context.Recipe.Include(x => x.RecipeList).FirstAsync(x => x.Id == id);
            _context.RecipeList.RemoveRange(recipe.RecipeList);
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();

            return Redirect("/AdminRecipes/Index");
        }
    }
}
