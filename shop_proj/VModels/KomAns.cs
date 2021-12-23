using shop_proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.VModels
{
    public class KomAns
    { public Tovar Tovar { get; set; }
        public List<Komentar> Komentars { get; set; }
    }
}
