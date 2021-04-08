using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class SerializableObject
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

    public class Serialization<T> where T : SerializableObject
    {
        public List<T> ReadList(string bytes)
        {
            return JsonConvert.DeserializeObject<List<T>>(bytes);
        }

        public string WriteList(List<T> recipes)
        {
            return JsonConvert.SerializeObject(recipes.ToArray());
        }
    }
}
