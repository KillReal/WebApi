using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using ModelLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Controllers
{
    public class RecipesController : Controller
    {
        private readonly MainDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public RecipesController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,  MainDbContext context)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public ModelLibrary.Recipe RecipeToExport(Models.Recipe recipe)
        {
            return new ModelLibrary.Recipe()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Weight = recipe.Weight,
                Colories = recipe.Colories,
                PictureCount = recipe.PictureList == null ? 0 : recipe.PictureList.Count() + 1,
                Proteins = recipe.Proteins,
                Greases = recipe.Greases,
                Carbohydrates = recipe.Carbohydrates,
                IsVegan = recipe.IsVegan,
                IsVegetarian = recipe.IsVegetarian,
                MainPicture = recipe.MainPicture,
                Type = (RecipeType)recipe.Type
            };
        }

        public async Task<String> Index()
        {
            _logger.LogInformation($"provided acces to /recipes by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");


            var list = await _context.Recipe.Include(x => x.PictureList).ToListAsync();
            List<ModelLibrary.Recipe> recipes = new List<ModelLibrary.Recipe>();
            foreach(var item in list)
            {
                recipes.Add(RecipeToExport(item));
            }

            return Serialization<ModelLibrary.Recipe>.WriteList(recipes);
        }

        public async Task<FileContentResult> ImageAsync(int id, int sid = -1)
        {
            _logger.LogInformation($"provided acces to /recipes/image/{id}?sid={sid} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");

            var recipe = await _context.Recipe.Include(x => x.PictureList).FirstOrDefaultAsync(m => m.Id == id);
            if (recipe != null)
            {
                byte[] pictureBytes = null;
                if (sid == -1)
                    pictureBytes = recipe.MainPicture;
                else if (sid < recipe.PictureList.Count())
                    pictureBytes = recipe.PictureList.ElementAt(sid).Picture;
                if (pictureBytes != null)
                    return new FileContentResult(pictureBytes, "image/jpeg");
            }
            return new FileContentResult(System.IO.File.ReadAllBytes("wwwroot/pics/noimg.jpg"), "image/jpeg");
        }

        public async Task<String> Details(int id)
        {
            _logger.LogInformation($"provided acces to /recipes/details/{id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");

            var recipe = await _context.Recipe.Include(x => x.PictureList).FirstOrDefaultAsync(x => x.Id == id);
            if (recipe == null)
                return null;

            return Serialization<ModelLibrary.Recipe>.Write(RecipeToExport(recipe));
        }

        public List<ModelLibrary.DayMenu> ToExportDayMenuList(List<Models.DayMenu> list)
        {
            var dayMenuList = new List<ModelLibrary.DayMenu>();
            foreach (var dayMenu in list)
            {
                var sDayMenu = new ModelLibrary.DayMenu()
                {
                    Id = dayMenu.Id,
                    Name = dayMenu.Name,
                    Date = dayMenu.Date.ToString("dd.MM.yyyy")
                };
                foreach (var recipeList in dayMenu.RecipeList)
                {
                    for (int i = 0; i < recipeList.DayUsage.Count(); i++)
                        if (recipeList.DayUsage[i])
                        {
                            switch (i)
                            {
                                case 0:
                                    sDayMenu.BreakfastRecipes.Add(RecipeToExport(recipeList.Recipe));
                                    break;
                                case 1:
                                    sDayMenu.LaunchRecipes.Add(RecipeToExport(recipeList.Recipe));
                                    break;
                                case 2:
                                    sDayMenu.DinnerRecipes.Add(RecipeToExport(recipeList.Recipe));
                                    break;
                            }
                        }
                }
                dayMenuList.Add(sDayMenu);
            }
            return dayMenuList;
        }

        public async Task<string> Menu()
        {
            _logger.LogInformation($"provided acces to /recipes/menu by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList)
                                                 .ThenInclude(x => x.Recipe)
                                                 .ThenInclude(x => x.PictureList)
                                                 .Where(x => x.Date > DateTime.Now.AddDays(-1))
                                                 .ToListAsync();
            return Serialization<ModelLibrary.DayMenu>.WriteList(ToExportDayMenuList(dayMenus));
        }

        public async Task<String> WeekMenu()
        {
            _logger.LogInformation($"provided acces to /recipes/weekmenu by user: {await _userManager.GetUserAsync(HttpContext.User)} " +
                $"[{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            int firstDay = (8 + (DateTime.Now.DayOfWeek - DayOfWeek.Monday)) % 7;
            int lastDay = 6 - firstDay;
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList)
                                                .ThenInclude(x => x.Recipe)
                                                .ThenInclude(x => x.PictureList)
                                                .Where(x => x.Date > DateTime.Now.AddDays(-1 * firstDay) && x.Date < DateTime.Now.AddDays(lastDay))
                                                .ToListAsync();
            return Serialization<ModelLibrary.DayMenu>.WriteList(ToExportDayMenuList(dayMenus));
        }
        public async Task<String> TodayMenuType(int type)
        {
            _logger.LogInformation($"provided acces to /recipes/todaymenutype/{type} by user: {await _userManager.GetUserAsync(HttpContext.User)} " +
                $"[{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            var dayMenu = await _context.DayMenu.Include(x => x.RecipeList)
                                                .ThenInclude(x => x.Recipe)
                                                .ThenInclude(x => x.PictureList)
                                                .FirstAsync(x => x.Date.Day == DateTime.Now.Day);
            var recipes = new List<ModelLibrary.Recipe>();
            foreach (var recipeList in dayMenu.RecipeList)
                if (recipeList.Recipe.Type == type)
                    recipes.Add(RecipeToExport(recipeList.Recipe));
            return Serialization<ModelLibrary.Recipe>.WriteList(recipes);
        }

        public async Task<String> TodayMenu()
        {
            _logger.LogInformation($"provided acces to /recipes/todaymenu by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");

            var dayMenu = await _context.DayMenu.Include(x => x.RecipeList)
                                                .ThenInclude(x => x.Recipe)
                                                .ThenInclude(x => x.PictureList)
                                                .FirstAsync(x => x.Date.Day == DateTime.Now.Day);

            if (dayMenu == null)
                return null;

            var sDayMenu = new ModelLibrary.DayMenu()
            {
                Id = dayMenu.Id,
                Name = dayMenu.Name,
                Date = dayMenu.Date.ToString("dd.MM.yyyy")
            };

            foreach (var recipeList in dayMenu.RecipeList)
            {
                for (int i = 0; i < recipeList.DayUsage.Count(); i++)
                    if (recipeList.DayUsage[i])
                    {
                        switch (i) 
                        { 
                            case 0:
                                sDayMenu.BreakfastRecipes.Add(RecipeToExport(recipeList.Recipe));
                                break;
                            case 1:
                                sDayMenu.LaunchRecipes.Add(RecipeToExport(recipeList.Recipe));
                                break;
                            case 2:
                                sDayMenu.DinnerRecipes.Add(RecipeToExport(recipeList.Recipe));
                                break;
                        }
                    }
            }

            return Serialization<ModelLibrary.DayMenu>.Write(sDayMenu);
        }
    }
}
