using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Category
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required]
        public string Name { get; set; }
        public List<Modell> Modells { get; set; }
        public Sex Sex { get; set; }

        public int SexId { get; set; }
      
    }
}
