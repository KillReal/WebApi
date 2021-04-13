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

        public List<Recipe> Recipe { get; set; }
        public List<DayMenu> DayMenu { get; set; }
    }
}
