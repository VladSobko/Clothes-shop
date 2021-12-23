using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace shop_proj.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string Name { get; set; }
    }
}
