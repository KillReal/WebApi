using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace WebApi.Models
{
    public class Recipe
    {
        [Key]
        public long Id { get; set; }    
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Range(0, 3000)]
        [Display(Name = "Вес")]
        public int Weight { get; set; }
        [Range(0, 5000)]
        [Display(Name = "Калории")]
        public int Colories { get; set; }
        [Display(Name = "Фотография")]
        public byte[] MainPicture { get; set; }
        [Display(Name = "Белки")]
        public int Proteins { get; set; }
        [Display(Name = "Жиры")]
        public int Greases { get; set; }
        [Display(Name = "Углеводы")]
        public int Carbohydrates { get; set; }
        [Display(Name = "Веганское")]
        public bool IsVegan { get; set; }
        [Display(Name = "Вегетарианское")]
        public bool IsVegetarian { get; set; }
        [Display(Name = "Тип блюда")]
        public int Type { get; set; }
        public List<RecipeList> RecipeList { get; set; }
        public List<PictureList> PictureList { get; set; }
    }
}