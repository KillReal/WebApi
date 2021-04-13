using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class DayMenu
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }

        public long RecipeListId { get; set; }
        public RecipeList RecipeList { get; set; }
    }
}
