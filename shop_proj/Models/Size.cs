using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Size
    {
        public int Id { get; set; }
        public Kind Kind { get; set; }
        public int KindId { get; set; }
         public String Name { get; set; }
        public int Count { get; set; }
        public Size(String name, int size)
        {
            this.Name = name;
            this.Count = size;
        }
        public Size()
        {
        }
    }

}
