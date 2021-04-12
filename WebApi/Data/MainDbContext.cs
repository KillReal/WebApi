using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;
using ModelsLibrary;

namespace WebApi.Data
{
    public class MainDbContext : IdentityDbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }
        public DbSet<Recipe> Recipe { get; set; }

        public DbSet<DayMenu> DayMenu { get; set; }
    }
}
