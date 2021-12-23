using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Order
    { 
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public List<Orderitem> Items { get; set; }
        public DateTime datezam { get; set; }
        public float Price { get; set; }
        public string Delivery { get; set; }
        public string City { get; set; }
        public string address { get; set; }
        public string postoffice { get; set; }
        public string Card_num { get; set; }

        public string Card_date { get; set; }
         public string Card_cvv { get; set; }

    }
}
