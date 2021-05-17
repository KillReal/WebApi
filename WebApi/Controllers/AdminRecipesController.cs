using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using static System.Net.Mime.MediaTypeNames;
using WebApi.Models;

namespace WebApi.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminRecipesController : Controller
    {
        private readonly MainDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        public AdminRecipesController(ILogger<HomeController> logger, MainDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<FileContentResult> ImageAsync(int id, int sid = -1)
        {
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

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"provided acces to /admin/index by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            return View(await _context.Recipe.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"provided acces to /admin/recipes/details{id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Weight,Colories,Proteins,Greases,Carbohydrates,HaveMeat")] Recipe recipe)
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
                            recipe.MainPicture = bytes;
                        }
                    }
                }
                else
                    recipe.MainPicture = System.IO.File.ReadAllBytes("wwwroot/pics/noimg.jpg");
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation($"provided acces to /admin/recipes/create/{recipe.Id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"provided acces to /admin/recipes/edit/{id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");

            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Weight,Colories,Proteins,Greases,Carbohydrates,HaveMeat")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

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
                            recipe.MainPicture = bytes;
                        }
                    }
                }
                else
                    recipe.MainPicture = (await _context.Recipe.AsNoTracking().FirstAsync(m => m.Id == id)).MainPicture;
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation($"provided acces to /admin/recipes/edit/{recipe.Id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"provided acces to /admin/recipes/delete/{id} by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var recipe = await _context.Recipe.FirstAsync(x => x.Id == id);
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(long id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}
