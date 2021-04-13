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
                    Image = item.Image,
                    Proteins = item.Proteins,
                    Greases = item.Greases,
                    Carbohydrates = item.Carbohydrates,
                    HaveMeat = item.HaveMeat
                });
            }

            return Serialization<ModelLibrary.Recipe>.WriteList(recipes);
        }
    }
}
