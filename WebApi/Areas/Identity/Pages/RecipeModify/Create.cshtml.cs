using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            public List<Boolean> RecipeInclude { get; set; } = new List<bool>();
        }

        public async Task<IActionResult> OnGet()
        {
            var DayMenu = await _context.DayMenu.ToListAsync();

            for (int i = 0; i < DayMenu.Count(); i++)
            {
                Input.DayMenuName.Add(DayMenu[i].Name);
                Input.RecipeInclude.Add(false);
            }
            if (DayMenu.Count() == 0)
            {
                for (int i = 0; i < Input.DayMenuName.Count(); i++)
                {
                    _context.Add(new DayMenu { Name = Input.DayMenuName[i], RecipeList = new List<RecipeList>() });
                }
                await _context.SaveChangesAsync();
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
                        if (Image != null && Image.Length > 0)
                        {
                            byte[] bytes;
                            using (var binaryReader = new BinaryReader(Image.OpenReadStream()))
                            {
                                bytes = binaryReader.ReadBytes((int)Image.Length);
                            }
                            Input.Recipe.Image = Tools.CorrectResolution(bytes);
                        }
                    }
                }
                else
                    Input.Recipe.Image = System.IO.File.ReadAllBytes("wwwroot/pics/noimg.jpg");

                var DayMenu = await _context.DayMenu.Include(x => x.RecipeList).ToListAsync();
                for (int i = 0; i < Input.DayMenuName.Count(); i++)
                {
                    if (Input.RecipeInclude[i])
                    {
                        DayMenu[i].RecipeList.Add(new RecipeList { DayMenu = DayMenu[i], Recipe = Input.Recipe });
                        _context.Add(DayMenu[i]);
                    }
                }
                _context.Add(Input.Recipe);
                _context.UpdateRange(DayMenu);
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
