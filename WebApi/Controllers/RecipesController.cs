using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;
using ModelsLibrary;

namespace WebApi.Controllers
{
    public class RecipesController : Controller
    {
        private readonly MainDbContext _context;

        public RecipesController(MainDbContext context)
        {
            _context = context;
        }

        public async Task<String> Index()
        {
            return Serialization<Recipe>.WriteList(await _context.Recipe.ToListAsync());
        }
    }
}
