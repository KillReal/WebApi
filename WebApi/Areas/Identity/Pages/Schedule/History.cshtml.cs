using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WebApi.Areas.Identity.Pages.Schedule
{
    public class HistoryModel : PageModel
    {
        private readonly MainDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ILogger<HomeController> _logger;

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public int Year;
            public int Week;
            public string MinDate;
            public string MaxDate;
            public string CurrentDate;
            public List<string> DayMenuName { get; set; } = new List<string>();
            public List<long> DayMenuId { get; set; } = new List<long>();
            public List<string> MenuTypeName { get; set; } = new List<string>();

            public List<List<List<Recipe>>> Recipes = new List<List<List<Recipe>>>();
        }

        public HistoryModel(MainDbContext context, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        private async Task UpdateModelAsync(int year, int week)
        {
            DateTime selectedWeek = Tools.GetDateFromWeek(year, week);
            Input.CurrentDate = $"{selectedWeek.Year}-W{Tools.GetWeekFromDate(selectedWeek)}";
            Input.Year = year;
            Input.Week = week;
            var historyDayMenus = await _context.DayMenu.Where(x => x.Date < DateTime.Now.AddDays(-1)).ToListAsync();
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList)
                                                 .ThenInclude(x => x.Recipe)
                                                 .Where(x => x.Date.Year == year && x.Date > selectedWeek
                                                                                 && x.Date < selectedWeek.AddDays(7)
                                                                                 && x.Date < DateTime.Now.AddDays(-1))
                                                 .OrderBy(x => x.Date).ToListAsync();
            Input.MinDate = $"{historyDayMenus.First().Date.Year}-W{Tools.GetWeekFromDate(historyDayMenus.First().Date)}";
            Input.MaxDate = $"{historyDayMenus.Last().Date.Year}-W{Tools.GetWeekFromDate(historyDayMenus.Last().Date)}";
            foreach (var dayMenu in dayMenus)
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
            Input.MenuTypeName.Add("Завтрак");
            Input.MenuTypeName.Add("Обед");
            Input.MenuTypeName.Add("Ужин");
        }

        public async Task<IActionResult> OnGetCreateFromHistory(long id)
        {
            _logger.LogInformation($"provided acces to /admin/schedule/createfromhistory={id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            var dayMenu = await _context.DayMenu.Include(x => x.RecipeList)
                                                .ThenInclude(x => x.Recipe)
                                                .FirstAsync(x => x.Id == id);
            var dayMenus = await _context.DayMenu.OrderBy(x => x.Date)
                                                 .Where(x => x.Date > DateTime.Now.AddDays(-1))
                                                 .ToListAsync();
            DateTime date;
            if (dayMenus.Count == 0)
                date = DateTime.Now;
            else
                date = dayMenus.Last().Date.AddDays(1);
            var newRecipeList = new List<RecipeList>();
            foreach (var recipeList in dayMenu.RecipeList)
            {
                newRecipeList.Add(new RecipeList()
                {
                    Recipe = recipeList.Recipe,
                    DayUsage = recipeList.DayUsage
                }) ;
            }
            var newDayMenu = new DayMenu()
            {
                Name = Tools.UpperLetter(date.ToString("dddd dd.MM")),
                RecipeList = newRecipeList,
                Date = date
            };
            _context.AddRange(newRecipeList);
            _context.Add(newDayMenu);
            await _context.SaveChangesAsync();
            return RedirectToPage("~/Schedule/Index");
        }

        public async Task<IActionResult> OnGet(int year = -1, int week = -1)
        {
            _logger.LogInformation($"provided acces to /admin/schedule/history by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            if (year == -1)
                year = DateTime.Now.Year;
            if (week == -1)
                week = Tools.GetWeekFromDate(DateTime.Now.AddDays(-1));
            await UpdateModelAsync(year, week);
            return Page();
        }

    }
}
