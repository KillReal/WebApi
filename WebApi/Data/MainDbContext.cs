using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<RecipeList> RecipeList { get; set; }
        public DbSet<DayMenu> DayMenu { get; set; }
    }
}
