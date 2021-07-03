using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.Schedule
{
    //[Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly MainDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ILogger<HomeController> _logger;

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public string SelectedDayMenuTab = "10";
            public List<string> DayMenuName { get; set; } = new List<string>();
            public List<long> DayMenuId { get; set; } = new List<long>();
            public List<string> MenuTypeName { get; set; } = new List<string>();

            public List<List<List<Recipe>>> Recipes = new List<List<List<Recipe>>>();
        }

        public IndexModel(MainDbContext context, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        private async Task UpdateModelAsync()
        {
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList).ThenInclude(x => x.Recipe).OrderBy(x => x.Date).ToListAsync();
            foreach (var dayMenu in dayMenus)
            {
                if (dayMenu.Date > DateTime.Now.AddDays(-1))
                {
                    List<List<Recipe>> dayRecipes = new List<List<Recipe>>() { };
                    dayRecipes.Add(new List<Recipe>());
                    dayRecipes.Add(new List<Recipe>());
                    dayRecipes.Add(new List<Recipe>());
                    foreach (var recipeList in dayMenu.RecipeList)
                    {
                        for (int i = 0; i < recipeList.DayUsage.Count(); i++)
                            if (recipeList.DayUsage[i])
                                dayRecipes[i].Add(recipeList.Recipe);
                    }
                    Input.Recipes.Add(dayRecipes);
                    Input.DayMenuName.Add(dayMenu.Name);
                    Input.DayMenuId.Add(dayMenu.Id);
                }
            }
            Input.MenuTypeName.Add("Завтрак");
            Input.MenuTypeName.Add("Обед");
            Input.MenuTypeName.Add("Ужин");
        }

        public async Task<IActionResult> OnGetDelete()
        {
            _logger.LogInformation($"provided acces to /admin/schedule/delete by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            var dayMenus = await _context.DayMenu.OrderBy(x => x.Date).Include(x => x.RecipeList)
                                                                      .Where(x => x.Date > DateTime.Now.AddDays(-1))
                                                                      .ToListAsync();
            if (dayMenus.Count > 0)
            {
                _context.RemoveRange(dayMenus.Last().RecipeList);
                _context.Remove(dayMenus.Last());
                await _context.SaveChangesAsync();
                await UpdateModelAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogInformation($"provided acces to /admin/schedule by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            await UpdateModelAsync();
            return Page();
        }
    }
}
