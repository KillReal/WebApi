using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ModelLibrary
{
    public abstract class SerializableObject
    {

    }

    public enum RecipeType
    {
        [Display(Name = "Неизвестно")]
        Unknown = -1,
        [Display(Name = "Первое")]
        Primary = 0,
        [Display(Name = "Второе")]
        Secondary = 1,
        [Display(Name = "Десерт")]
        Desert = 2,
        [Display(Name = "Салат")]
        Salad = 3,
        [Display(Name = "Напиток")]
        Drink = 4,

    }

    public static class Tools 
    {
        public static string GetEnumName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }

    public class Recipe : SerializableObject
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        [Range(1, 3000)]
        public int Weight { get; set; }
        [Range(1, 5000)]
        public int Colories { get; set; }
        [Range(1, 5000)]
        public int Proteins { get; set; }
        [Range(1, 5000)]
        public int Greases { get; set; }
        [Range(1, 5000)]
        public int Carbohydrates { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarian { get; set; }
        public int PictureCount { get; set; }
        public RecipeType Type { get; set; }
    }

    public class DayMenu : SerializableObject
    { 
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public String Date { get; set; }
        public List<long> BreakfastRecipes { get; set; } = new List<long>();
        public List<long> LaunchRecipes { get; set; } = new List<long>();
        public List<long> DinnerRecipes { get; set; } = new List<long>();
    }

    public static class Serialization<T> where T : SerializableObject
    {
        public static List<T> ReadList(string bytes)
        {
            return JsonConvert.DeserializeObject<List<T>>(bytes);
        }

        public static string WriteList(List<T> recipes)
        {
            return JsonConvert.SerializeObject(recipes.ToArray());
        }

        public static T Read(string bytes)
        {
            return JsonConvert.DeserializeObject<T>(bytes);
        }

        public static string Write(T recipe)
        {
            return JsonConvert.SerializeObject(recipe);
        }
    }
}
