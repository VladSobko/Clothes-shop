using shop_proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.VModels
{
    public class KorzynaOrder
    {
        public Korzyna korzyna { get; set; }
        public Order order { get; set; }
        public List<Orderitem> orderitems { get; set; }

        public string items { get; set; }
    }
}
