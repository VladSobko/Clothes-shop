using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Modell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tovar> Tovars { get; set; }
        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
