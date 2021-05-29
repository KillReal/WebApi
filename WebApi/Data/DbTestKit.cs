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
            Name = "Салат из капусты с морковью",
            Weight = 100,
            Colories = 50,
            Type = 3
            },
            new Recipe
            {
            Name = "Винегрет с горошком",
            Weight = 120,
            Colories = 99,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Астра",
            Weight = 100,
            Colories = 209,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Бриз",
            Weight = 100,
            Colories = 184,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Мечта",
            Weight = 100,
            Colories = 231,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Анютка",
            Weight = 100,
            Colories = 190,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Осень",
            Weight = 100,
            Colories = 85,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Орион",
            Weight = 100,
            Colories = 268,
            Type = 3
            },
            new Recipe
            {
            Name = "Салат Улыбка",
            Weight = 100,
            Colories = 118,
            Type = 3
            },
            new Recipe
            {
            Name = "Суп с куриным филе и вермишелью",
            Weight = 275,
            Colories = 149,
            Type = 0
            },
            new Recipe
            {
            Name = "Суп из овощей",
            Weight = 250,
            Colories = 36,
            Type = 0
            },
            new Recipe
            {
            Name = "Солянка",
            Weight = 300,
            Colories = 189,
            Type = 0
            },
            new Recipe
            {
            Name = "Борщ",
            Weight = 250,
            Colories = 100,
            Type = 0
            },
            new Recipe
            {
            Name = "Пикша жареная",
            Weight = 100,
            Colories = 109,
            Type = 1
            },
            new Recipe
            {
            Name = "Котлета рыбная",
            Weight = 100,
            Colories = 89,
            Type = 1
            },
            new Recipe
            {
            Name = "Язык отварной",
            Weight = 50,
            Colories = 115,
            Type = 1
            },
            new Recipe
            {
            Name = "Говядина тварная",
            Weight = 50,
            Colories = 129,
            Type = 1
            },
            new Recipe
            {
            Name = "Стейк из свинины",
            Weight = 110,
            Colories = 259,
            Type = 1
            },
            new Recipe
            {
            Name = "Каша овсяная",
            Weight = 200,
            Colories = 204,
            Type = 1
            },
            new Recipe
            {
            Name = "Омлет с сыром",
            Weight = 120,
            Colories = 159,
            Type = 1
            },
            new Recipe
            {
            Name = "Сырники",
            Weight = 120,
            Colories = 109,
            Type = 1
            },
            new Recipe
            {
            Name = "Тефтели в подливе",
            Weight = 120,
            Colories = 177,
            Type = 1
            },
            new Recipe
            {
            Name = "Свинина тушеная в соусе",
            Weight = 100,
            Colories = 229,
            Type = 1
            },
            new Recipe
            {
            Name = "Свиниа по-гусарски",
            Weight = 110,
            Colories = 163,
            Type = 1
            },
            new Recipe
            {
            Name = "Плов из шейки",
            Weight = 250,
            Colories = 393,
            Type = 1
            },
            new Recipe
            {
            Name = "Поджарка из куриного филе",
            Weight = 105,
            Colories = 129,
            Type = 1
            },
            new Recipe
            {
            Name = "Филе курицы по-охотничьи",
            Weight = 150,
            Colories = 258,
            Type = 1
            },
            new Recipe
            {
            Name = "Филе курицы по-мулянски",
            Weight = 100,
            Colories = 157,
            Type = 1
            },
            new Recipe
            {
            Name = "Филе курицы с ананасами",
            Weight = 120,
            Colories = 189,
            Type = 1
            },
            new Recipe
            {
            Name = "Люля кебаб",
            Weight = 80,
            Colories = 192,
            Type = 1
            },
            new Recipe
            {
            Name = "Зразы ленивые",
            Weight = 100,
            Colories = 205,
            Type = 1
            },
            new Recipe
            {
            Name = "Котлеты морковные",
            Weight = 100,
            Colories = 151,
            Type = 1
            },
            new Recipe
            {
            Name = "Оладьи",
            Weight = 150,
            Colories = 309,
            Type = 1
            },
            new Recipe
            {
            Name = "Пюре картофельное",
            Weight = 200,
            Colories = 176,
            Type = 2
            },
            new Recipe
            {
            Name = "Пюре гороховое",
            Weight = 200,
            Colories = 182,
            Type = 2
            },
            new Recipe
            {
            Name = "Рожки отварные",
            Weight = 200,
            Colories = 224,
            Type = 2
            },
            new Recipe
            {
            Name = "Каша перловая с овощами",
            Weight = 150,
            Colories = 150,
            Type = 2
            },
            new Recipe
            {
            Name = "Каша гречневая рассыпчатая",
            Weight = 150,
            Colories = 168,
            Type = 2
            },
            new Recipe
            {
            Name = "Компот из винограда",
            Weight = 200,
            Colories = 154,
            Type = 4
            },
            new Recipe
            {
            Name = "Чай зеленый",
            Weight = 200,
            Colories = 0,
            Type = 4
            },
            new Recipe
            {
            Name = "Чай черный",
            Weight = 200,
            Colories = 0,
            Type = 4
            },
            new Recipe
            {
            Name = "Кофе 3в1",
            Weight = 200,
            Colories = 120,
            Type = 4
            },
            new Recipe
            {
            Name = "Вода",
            Weight = 200,
            Colories = 0,
            Type = 4
            }
            };
            foreach (var item in recipeList)
                context.Set<Recipe>().AddIfNotExists(item, x => x.Name == item.Name);
            context.SaveChanges();
        }
    }
}
