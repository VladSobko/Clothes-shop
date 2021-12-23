using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Tovar
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть назву")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Довжина рядка повинна бути від 3 до 300 символів")]
        public string Name { get; set; }
        public int ModellId { get; set; }
        public Modell Modell { get; set; }
        [Required(ErrorMessage = "Введіть бренд")]
        public string Brend { get; set; }
        [Required(ErrorMessage = "Введіть матеріал")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Введіть ціну")]
        public double Price { get; set; }
        public List<Kind> Kinds { get; set; }
        public List<Komentar> Komentars { get; set; }

    }
    
}
