using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.RecipeModify
{
    public class EditModel : PageModel
    {
        private readonly MainDbContext _context;

        public EditModel(MainDbContext context)
        {
            _context = context;
        }

        public string ReturnUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            public Recipe Recipe { get; set; } = new Recipe();
            public List<string> DayMenuName { get; set; } = new List<string>();
            public List<Boolean> RecipeInclude { get; set; } = new List<bool>();
        }

        public async Task<IActionResult> OnGet(long id, string returnUrl = null)
        {
            Input.Recipe = await _context.Recipe.Include(x => x.RecipeList)
                                         .ThenInclude(x => x.DayMenu)
                                         .FirstOrDefaultAsync(x => x.Id == id);
            var dayMenus = await _context.DayMenu.Include(x => x.RecipeList).ToListAsync();
            for (int i = 0; i < dayMenus.Count(); i++)
            {
                Input.DayMenuName.Add(dayMenus[i].Name);
                Input.RecipeInclude.Add(dayMenus[i].RecipeList.Contains(Input.Recipe.RecipeList.Find(x => x.DayMenu.Id == dayMenus[i].Id)));
            }
            ReturnUrl = returnUrl;
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
                    Input.Recipe.Image = (await _context.Recipe.AsNoTracking().FirstAsync(m => m.Id == Input.Recipe.Id)).Image;
                _context.Update(Input.Recipe);

                var DayMenu = await _context.DayMenu.Include(x => x.RecipeList).ToListAsync();
                Input.Recipe = _context.Recipe.Include(x => x.RecipeList).First(x => x.Id == Input.Recipe.Id);
                for (int i = 0; i < DayMenu.Count(); i++)
                {
                    var recipeList = Input.Recipe.RecipeList.Find(x => x.DayMenu.Id == DayMenu[i].Id);
                    if (recipeList != null)
                    {
                        if (Input.RecipeInclude[i] == false)
                            _context.Remove(recipeList);
                    }
                    else if (Input.RecipeInclude[i])
                    {
                        _context.Add(new RecipeList { DayMenu = DayMenu[i], Recipe = Input.Recipe });
                    }
                }
                _context.Update(Input.Recipe);
                await _context.SaveChangesAsync();
            }
            else
            {
                var query = from state in ModelState.Values
                            from error in state.Errors
                            select error.ErrorMessage;
                string s = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
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
