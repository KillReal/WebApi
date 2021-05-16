using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.RecipeModify
{
    public class DeleteModel : PageModel
    {
        private readonly MainDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ILogger<HomeController> _logger;

        public DeleteModel(MainDbContext context, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGet(long id)
        {
            _logger.LogInformation($"provided acces to /admin/recipes/delete?get?id={id} by user: {await _userManager.GetUserAsync(HttpContext.User)}");
            Recipe = await _context.Recipe.Include(x => x.RecipeList)
                                        .FirstOrDefaultAsync(x => x.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            _logger.LogInformation($"provided acces to /admin/recipes/delete?post?id={id} by user: {await _userManager.GetUserAsync(HttpContext.User)}");
            var recipe = await _context.Recipe.Include(x => x.RecipeList)
                                              .Include(x => x.PictureList)
                                              .FirstAsync(x => x.Id == id);
            _context.RemoveRange(recipe.RecipeList);
            _context.RemoveRange(recipe.PictureList);
            _context.Remove(recipe);
            await _context.SaveChangesAsync();

            return Redirect("/AdminRecipes/Index");
        }
    }
}
