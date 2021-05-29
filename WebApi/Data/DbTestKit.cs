using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public static class DbTestKit
    {
        internal static void Initialize(MainDbContext context)
        {
            context.Database.EnsureCreated();
            var recipeList = new List<Recipe>() {
            new Recipe
            {
                Name = "Test111",
                Weight = 150,
                Colories = 100
            },
            new Recipe
            {
                Name = "Test222",
                Weight = 1337
            } };
            foreach (var item in recipeList)
                context.Set<Recipe>().AddIfNotExists(item, x => x.Name == item.Name);
            context.SaveChanges();
        }
    }
}
