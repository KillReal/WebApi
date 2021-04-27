using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.RecipeModify
{
    public class CreateModel : PageModel
    {
        private readonly MainDbContext _context;
        private IConfiguration Configuration { get; }

        public CreateModel(MainDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public Recipe Recipe { get; set; } = new Recipe();
            public List<String> DayMenuName = new List<string>();
            public int MainPictureId { get; set; }
            public List<List<Boolean>> RecipeDayInclude { get; set; } = new List<List<bool>>();
        }

        public async Task<IActionResult> OnGet()
        {
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList)
                                                 .ThenInclude(x => x.Recipe)
                                                 .ToListAsync();

            for (int i = 0; i < dayMenus.Count(); i++)
            {
                Input.DayMenuName.Add(dayMenus[i].Name);
                Input.RecipeDayInclude.Add(new List<bool>());
                var recipeList = dayMenus[i].RecipeList.Find(x => x.DayMenu.Id == dayMenus[i].Id && x.Recipe.Id == Input.Recipe.Id);
                if (recipeList == null)
                    recipeList = new RecipeList();
                foreach (var usage in recipeList.DayUsage)
                {
                    Input.RecipeDayInclude[i].Add(usage);
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    foreach (var Image in files)
                    {
                        
                    }
                    for (int i = 0; i < files.Count(); i++)
                    {
                        if (files[i] != null && files[i].Length > 0)
                        {
                            byte[] bytes;
                            using (var binaryReader = new BinaryReader(files[i].OpenReadStream()))
                            {
                                bytes = binaryReader.ReadBytes((int)files[i].Length);
                            }
                            if (i == Input.MainPictureId)
                                Input.Recipe.MainPicture = Tools.CorrectResolution(bytes);
                            else
                            {
                                PictureList pictureList = new PictureList()
                                {
                                    Picture = Tools.CorrectResolution(bytes),
                                    Recipe = Input.Recipe
                                };
                                _context.Add(pictureList);
                            }
                        }
                    }
                }
                else
                    Input.Recipe.MainPicture = System.IO.File.ReadAllBytes("wwwroot/pics/noimg.jpg");
                _context.Add(Input.Recipe);
                await _context.SaveChangesAsync();
                Input.Recipe = _context.Recipe.First(x => x.Name == Input.Recipe.Name);

                var DayMenu = await _context.DayMenu.Include(x => x.RecipeList).ToListAsync();
                Input.Recipe = _context.Recipe.Include(x => x.RecipeList).First(x => x.Id == Input.Recipe.Id);
                for (int i = 0; i < DayMenu.Count(); i++)
                {
                    if (Input.RecipeDayInclude[i].Contains(true))
                    {
                        var newRecipeList = new RecipeList()
                        {
                            Recipe = Input.Recipe,
                            DayMenu = DayMenu[i],
                            DayUsage = Input.RecipeDayInclude[i].ToArray()
                        };
                        _context.Add(newRecipeList);
                    }
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                var query = from state in ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;

                ErrorViewModel errorList = new ErrorViewModel() { RequestId = query.ToString() };
                return Redirect("/Home/Error");
            }
            
            return Redirect("/AdminRecipes/Index");
        }

        private bool RecipeExists(long id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}
