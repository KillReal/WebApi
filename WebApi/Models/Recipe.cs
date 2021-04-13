using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebApi.Models
{
    public class Recipe
    {
        [Key]
        public long Id { get; set; }    
        public string Name { get; set; }
        [Range(1, 3000)]
        public int Weight { get; set; }
        [Range(1, 5000, ErrorMessage = "kek")]
        public int Colories { get; set; }
        public byte[] Image { get; set; }
        public int Proteins { get; set; }
        public int Greases { get; set; }
        public int Carbohydrates { get; set; }
        public bool HaveMeat { get; set; }

        public RecipeList RecipeList { get; set; }
    }
}