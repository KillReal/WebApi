using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
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
        public byte[] Image { get; set; }
    }

    public class DayMenu : SerializableObject
    { 
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Recipe> recipes { get; set; }
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
    }
}
