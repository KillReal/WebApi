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
        [Display(Name = "��������")]
        public string Name { get; set; }
        [Range(1, 3000)]
        [Display(Name = "���")]
        public int Weight { get; set; }
        [Range(1, 5000)]
        [Display(Name = "�������")]
        public int Colories { get; set; }
        [Display(Name = "����������")]
        public byte[] MainPicture { get; set; }
        [Display(Name = "�����")]
        public int Proteins { get; set; }
        [Display(Name = "����")]
        public int Greases { get; set; }
        [Display(Name = "��������")]
        public int Carbohydrates { get; set; }
        [Display(Name = "��������������")]
        public bool HaveMeat { get; set; }
        public List<RecipeList> RecipeList { get; set; }

        public List<PictureList> PictureList { get; set; }
    }
}