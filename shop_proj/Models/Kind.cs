using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Kind
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введіть колір")]
        public string Colour { get; set; }
        public Tovar Tovar { get; set; }
        public int TovarId { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Image> Images { get; set; }
    
    }
}
