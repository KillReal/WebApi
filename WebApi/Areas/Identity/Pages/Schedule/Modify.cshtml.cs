using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.Schedule
{
    public class ModifyModel : PageModel
    {
        private readonly MainDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private ILogger<HomeController> _logger;
        private IConfiguration Configuration { get; }

        public ModifyModel(MainDbContext context, IConfiguration configuration, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            Configuration = configuration;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public bool ModifyType { get; set; } = false;
            public DayMenu DayMenu { get; set; } = new DayMenu();

            public List<List<List<bool>>> RecipeUsageList { get; set; } = new List<List<List<bool>>>();

            public List<List<Recipe>> RecipeList = new List<List<Recipe>>();

            public List<string> TypeNameList = new List<string>();
            public int RecipeTypeCount { set; get; }

        }

        public async Task<IActionResult> OnGet(int id = -1, string returnUrl = null)
        {
            Input.RecipeTypeCount = Enum.GetNames(typeof(ModelLibrary.RecipeType)).Length;
            for (int i = 0; i < Input.RecipeTypeCount; i++)
            {
                Input.RecipeUsageList.Add(new List<List<bool>>());
                Input.RecipeList.Add(new List<Recipe>());
                Input.TypeNameList.Add(ModelLibrary.Tools.GetEnumName((ModelLibrary.RecipeType)i));
            }
            _logger.LogInformation($"provided acces to /admin/recipes/modify?get&id={id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            if (DayMenuExists(id))
            {
                Input.DayMenu = _context.DayMenu.Where(x => x.Id == id).Include(x => x.RecipeList).FirstOrDefault();
                Input.ModifyType = true;
            }
            else
            {
                var dayMenus = await _context.DayMenu.OrderBy(x => x.Date)
                                                 .Where(x => x.Date > DateTime.Now.AddDays(-1))
                                                 .ToListAsync();
                DateTime date;
                if (dayMenus.Count == 0)
                    date = DateTime.Now;
                else
                    date = dayMenus.Last().Date.AddDays(1);
                Input.DayMenu = new DayMenu()
                {
                    Date = date,
                    Name = Tools.UpperLetter(date.ToString("dddd dd.MM")),
                    RecipeList = new List<RecipeList>()
                };
            }
            var recipes = await _context.Recipe.Include(x => x.RecipeList).OrderBy(x => x.Id).ToListAsync();
            foreach (var recipe in recipes)
            {
                var usage = (await _context.RecipeList.Where(x => x.DayMenu.Id == Input.DayMenu.Id && x.Recipe.Id == recipe.Id)
                                                                 .FirstOrDefaultAsync());
                List<bool> UsageList = new List<bool>() { false, false, false };
                if (usage != null)
                    UsageList = usage.DayUsage.ToList();
                Input.RecipeList[recipe.Type].Add(recipe);
                Input.RecipeUsageList[recipe.Type].Add(UsageList);
            }
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation($"provided acces to /admin/recipes/modify?post?id={Input.DayMenu.Id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            var recipes = await _context.Recipe.OrderBy(x => x.Id).ToListAsync();
            if (ModelState.IsValid)
            {
                if (DayMenuExists(Input.DayMenu.Id))
                {
                    Input.DayMenu.RecipeList = await _context.RecipeList.Where(x => x.DayMenu.Id == Input.DayMenu.Id).ToListAsync();
                    _context.RemoveRange(Input.DayMenu.RecipeList);
                    _context.DayMenu.Update(Input.DayMenu);
                }
                else
                    _context.DayMenu.Add(Input.DayMenu);
                List<RecipeList> recipeList = new List<RecipeList>();
                foreach (var recipe in recipes)
                {
                    recipeList.Add(new RecipeList() { Recipe = recipe, DayMenu = Input.DayMenu, DayUsage = Input.RecipeUsageList[recipe.Type].First().ToArray() });
                    Input.RecipeUsageList[recipe.Type].RemoveAt(0);
                }
                _context.AddRange(recipeList);
                _context.SaveChanges();
            }
            return RedirectToAction("Schedule", "Identity");
        }

        private bool DayMenuExists(long id)
        {
            return _context.DayMenu.Any(e => e.Id == id);
        }
    }
}
