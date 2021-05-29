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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Identity.Pages.RecipeModify
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
            public Recipe Recipe { get; set; } = new Recipe();
            public List<SelectListItem> Type { get; set; } = new List<SelectListItem>();
            public int MainPictureId { get; set; }
        }

        public async Task<IActionResult> OnGet(int id = -1, string returnUrl = null)
        {
            _logger.LogInformation($"provided acces to /admin/recipes/modify?get&id={id} by user: " +
                $"{await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            if (RecipeExists(id))
            {
                Input.Recipe = _context.Recipe.Where(x => x.Id == id).Include(x => x.PictureList).FirstOrDefault();
                Input.ModifyType = true;
            }
            else
                Input.Recipe.Name = "Название";
            List<ModelLibrary.RecipeType> enums = new List<ModelLibrary.RecipeType>(Enum.GetValues(typeof(ModelLibrary.RecipeType)).Cast<ModelLibrary.RecipeType>().ToList());
            foreach (var item in enums)
            {
                Input.Type.Add(new SelectListItem
                {
                    Value = ((int)item).ToString(),
                    Text = ModelLibrary.Tools.GetEnumName(item)
                });
            }
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation($"provided acces to /admin/recipes/modify?post?id={Input.Recipe.Id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            if (ModelState.IsValid)
            {
                List<PictureList> pictureList = new List<PictureList>();
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count(); i++)
                    {
                        if (files[i] != null && files[i].Length > 0)
                        {
                            byte[] bytes;
                            using (var binaryReader = new BinaryReader(files[i].OpenReadStream()))
                            {
                                bytes = binaryReader.ReadBytes((int)files[i].Length);
                            }
                            if (i == Input.MainPictureId - 1)
                                Input.Recipe.MainPicture = Tools.CorrectResolution(bytes);
                            else
                            {
                                var Picture = new PictureList()
                                {
                                    Picture = Tools.CorrectResolution(bytes),
                                    Recipe = Input.Recipe
                                };
                                pictureList.Add(Picture);
                            }
                        }
                    }
                }
                if (RecipeExists(Input.Recipe.Id))
                {
                    if (files.Count == 0)
                        Input.Recipe.MainPicture = (await _context.Recipe.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Input.Recipe.Id)).MainPicture;
                    Input.Recipe.PictureList = (await _context.Recipe.AsNoTracking().Include(x => x.PictureList)
                                                                                    .FirstAsync(x => x.Id == Input.Recipe.Id))
                                                                                    .PictureList;
                    _context.Update(Input.Recipe);
                    Input.Recipe = await _context.Recipe.Include(x => x.PictureList).FirstAsync(x => x.Id == Input.Recipe.Id);
                    if (pictureList.Count > 0)
                    {
                        _context.RemoveRange(Input.Recipe.PictureList);
                        _context.AddRange(pictureList);
                    }
                }
                else
                {
                    if (Input.Recipe.MainPicture == null)
                        Input.Recipe.MainPicture = System.IO.File.ReadAllBytes("wwwroot/pics/noimg.jpg");
                    Input.Recipe.PictureList = pictureList;
                    _context.AddRange(pictureList);
                    _context.Add(Input.Recipe);
                }
                await _context.SaveChangesAsync();
            }     
            return Redirect("/AdminRecipes/Index");
        }

        private bool RecipeExists(long id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}
