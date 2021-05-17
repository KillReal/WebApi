using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Controllers
{
    public class AdminScheduleController : Controller
    {
        private readonly MainDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        public AdminScheduleController(ILogger<HomeController> logger, MainDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            _logger.LogInformation($"provided acces to /admin/schedule/index by user: {await _userManager.GetUserAsync(HttpContext.User)} [{DateTime.Now}] {HttpContext.Connection.RemoteIpAddress}");
            return View("Schedule/Index");
        }
    }
}
