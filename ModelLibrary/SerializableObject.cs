﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelLibrary
{
    public abstract class SerializableObject
    {

    }
    public class Recipe : SerializableObject
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        [Range(1, 3000)]
        public int Weight { get; set; }
        [Range(1, 5000, ErrorMessage = "kek")]
        public int Colories { get; set; }
        public int Proteins { get; set; }
        public int Greases { get; set; }
        public int Carbohydrates { get; set; }
        public bool HaveMeat { get; set; }
        public int PictureCount { get; set; }
    }

    public class DayMenu : SerializableObject
    { 
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public List<long> BreakfastRecipes { get; set; } = new List<long>();
        public List<long> LaunchRecipes { get; set; } = new List<long>();
        public List<long> DinnerRecipes { get; set; } = new List<long>();
    }

    public static class Serialization<T> where T : SerializableObject
    {
        // Использование: List<Recipe> list = Serialization<Recipe>.ReadList(bytes);
        public static List<T> ReadList(string bytes)
        {
            return JsonConvert.DeserializeObject<List<T>>(bytes);
        }
        // Использование: string bytes = Serialization<Recipe>.WriteList(List<Recipe>);
        public static string WriteList(List<T> recipes)
        {
            return JsonConvert.SerializeObject(recipes.ToArray());
        }

        // Использование: Recipe list = Serialization<Recipe>.Read(bytes);
        public static T Read(string bytes)
        {
            return JsonConvert.DeserializeObject<T>(bytes);
        }
        // Использование: string bytes = Serialization<Recipe>.Write(Recipe);
        public static string Write(T recipe)
        {
            return JsonConvert.SerializeObject(recipe);
        }
    }
}
