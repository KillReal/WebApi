using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class PictureList
    {
        [Key]
        public long Id { get; set; }
        public Recipe Recipe { get; set; }

        public byte[] Picture { get; set; }
    }
}
