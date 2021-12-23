using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Korzynaitem
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        public int SizeId { get; set; }
        public int Count { get; set; }
        public Korzyna Korzyna { get; set; }
        public int KorzynaId { get; set; }
    }
}
