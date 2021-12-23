using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Korzyna
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public List<Korzynaitem> Items { get; set; }

       
    }
}
