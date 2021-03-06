using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RecipeList
    {
        [Key]
        public long Id { get; set; }
        public bool[] DayUsage { get; set; } = new bool[3] { false, false, false };
        public Recipe Recipe { get; set; }
        public DayMenu DayMenu { get; set; }
    }
}
