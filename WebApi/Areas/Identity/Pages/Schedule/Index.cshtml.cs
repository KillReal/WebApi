using System;
using System.Collections.Generic;
using System.Globalization;
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
            public string SelectedDayMenuTab = "10";
            public bool isHistory { get; set; } = false;
            public List<string> DayMenuName { get; set; } = new List<string>();
            public List<long> DayMenuId { get; set; } = new List<long>();
            public List<string> MenuTypeName { get; set; } = new List<string>();

            public List<List<List<Recipe>>> Recipes = new List<List<List<Recipe>>>();
        }

        public IndexModel(MainDbContext context)
        {
            _context = context;
        }

        private async Task UpdateModelAsync(bool isHistory = false)
        {
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList).ThenInclude(x => x.Recipe).OrderBy(x => x.Date).ToListAsync();
            foreach (var dayMenu in dayMenus)
            {
                if (isHistory && (dayMenu.Date - DateTime.Now).TotalDays < 0
                    || 
                    !isHistory && (dayMenu.Date - DateTime.Now).TotalDays >= 0)
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

        public async Task<IActionResult> OnGetCreateFromHistory(long id)
        {
            var dayMenu = await _context.DayMenu.Include(x => x.RecipeList)
                                                .FirstAsync(x => x.Id == id);
            var dayMenus = await _context.DayMenu.OrderBy(x => x.Date)
                                                 .Where(x => x.Date >= DateTime.Now.AddDays(-1))
                                                 .ToListAsync();
            DateTime date;
            if (dayMenus.Count == 0)
                date = DateTime.Now;
            else
                date = dayMenus.Last().Date.AddDays(1);
            var newDayMenu = new DayMenu()
            {
                Name = Tools.UpperLetter(date.ToString("dddd dd.MM")),
                RecipeList = dayMenu.RecipeList,
                Date = date,
            };
            _context.Add(newDayMenu);
            await _context.SaveChangesAsync();
            await UpdateModelAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetHistory()
        {
            await UpdateModelAsync(true);
            Input.isHistory = true;
            return Page();
        }

        public async Task<IActionResult> OnGetCreate(InputModel input)
        {
            Input = input;
            var dayMenus = await _context.DayMenu.OrderBy(x => x.Date)
                                                 .Where(x => x.Date >= DateTime.Now.AddDays(-1))
                                                 .ToListAsync();
            DateTime date;
            if (dayMenus.Count == 0)
                date = DateTime.Now;
            else
                date = dayMenus.Last().Date.AddDays(1);
            var dateTimeFormats = new CultureInfo("Ru-ru").DateTimeFormat;
            DayMenu dayMenu = new DayMenu()
            {
                Date = date,
                Name = Tools.UpperLetter(date.ToString("dddd dd.MM")),
                RecipeList = new List<RecipeList>()
            };
            await _context.AddAsync(dayMenu);
            await _context.SaveChangesAsync();
            await UpdateModelAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetDelete()
        {
            var dayMenus = await _context.DayMenu.OrderBy(x => x.Date).Include(x => x.RecipeList)
                                                                      .Where(x => x.Date >= DateTime.Now.AddDays(-1))
                                                                      .ToListAsync();
            if (dayMenus.Count > 0)
            {
                _context.RemoveRange(dayMenus.Last().RecipeList);
                _context.Remove(dayMenus.Last());
                await _context.SaveChangesAsync();
                await UpdateModelAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnGet()
        {
            await UpdateModelAsync();
            return Page();
        }
    }
}
