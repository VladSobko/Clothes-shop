using shop_proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.VModels
{
    public class IndexModel
    {
        public IEnumerable<Tovar> Tovars { get; set; }
        public PageModel PageModel { get; set; }
    }
}
