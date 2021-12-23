using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using shop_proj.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
   public interface Irep
    {
        public AppContext context { get; set; }
        public ILogger<UserController> logger { get; set; }
        public UserManager<User> userManager { get; set; }
    }
}
