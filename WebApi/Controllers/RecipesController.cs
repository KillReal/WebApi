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

        public async Task<String> Index()
        {
            _logger.LogInformation($"provided acces to /recipes by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.UtcNow}]");

            var list = await _context.Recipe.ToListAsync();
            List<ModelLibrary.Recipe> recipes = new List<ModelLibrary.Recipe>();
            foreach(var item in list)
            {
                recipes.Add(new ModelLibrary.Recipe
                {
                    Id = item.Id,
                    Name = item.Name,
                    Weight = item.Weight,
                    Colories = item.Colories,
                    Image = null,
                    Proteins = item.Proteins,
                    Greases = item.Greases,
                    Carbohydrates = item.Carbohydrates,
                    HaveMeat = item.HaveMeat
                });
            }

            return Serialization<ModelLibrary.Recipe>.WriteList(recipes);
        }

        public FileContentResult Image(int id, int sid = -1)
        {
            var recipe = _context.Recipe.Include(x => x.PictureList).First(m => m.Id == id);
            byte[] pictureBytes;
            if (sid == -1)
                pictureBytes = recipe.MainPicture;
            else
                pictureBytes = recipe.PictureList.ElementAt(sid).Picture;
            return pictureBytes != null
                ? new FileContentResult(pictureBytes, "image/jpeg")
                : null;
        }

        public async Task<String> Details(int id)
        {
            _logger.LogInformation($"provided acces to /recipes/details/{id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.UtcNow}]");

            var recipe = await _context.Recipe.FirstOrDefaultAsync(x => x.Id == id);
            if (recipe == null)
                return null;
            var sRecipe = new ModelLibrary.Recipe()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Weight = recipe.Weight,
                Colories = recipe.Colories,
                Image = recipe.MainPicture,
                Proteins = recipe.Proteins,
                Greases = recipe.Greases,
                Carbohydrates = recipe.Carbohydrates,
                HaveMeat = recipe.HaveMeat
            };

            return Serialization<ModelLibrary.Recipe>.Write(sRecipe);
        }

        public async Task<String> TodayMenu()
        {
            _logger.LogInformation($"provided acces to /recipes/todaymenu by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.UtcNow}]");

            var dayMenu = await _context.DayMenu.Include(x => x.RecipeList)
                                                .ThenInclude(x => x.Recipe)
                                                .FirstAsync(x => x.Date.Day == DateTime.Now.Day);

            if (dayMenu == null)
                return null;

            var sDayMenu = new ModelLibrary.DayMenu()
            {
                Id = dayMenu.Id,
                Name = dayMenu.Name
            };

            foreach (var recipeList in dayMenu.RecipeList)
            {
                for (int i = 0; i < recipeList.DayUsage.Count(); i++)
                    if (recipeList.DayUsage[i])
                    {
                        switch (i) 
                        {
                            case 0:
                                sDayMenu.BreakfastRecipes.Add(recipeList.Recipe.Id);
                                break;
                            case 1:
                                sDayMenu.LaunchRecipes.Add(recipeList.Recipe.Id);
                                break;
                            case 2:
                                sDayMenu.DinnerRecipes.Add(recipeList.Recipe.Id);
                                break;
                        }
                    }
            }

            return Serialization<ModelLibrary.DayMenu>.Write(sDayMenu);
        }
    }
}
