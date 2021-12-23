using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Orderitem
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Size Size { get; set; }
        public int SizeId { get; set; }
    }
}
