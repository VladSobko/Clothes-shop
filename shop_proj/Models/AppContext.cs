using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace shop_proj.Models
{
    
    public class AppContext : IdentityDbContext<User>
    {
        public DbSet<Tovar> Tovars { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Orderitem> Orderitems { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Korzyna> Korzuna { get; set; }
        public DbSet<Korzynaitem> Korzynaitems { get; set; }
        public DbSet<Sex> Sexs { get; set; }
        public DbSet<Modell> Modells { get; set; }
        public DbSet<Komentar> Komentars { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public AppContext(DbContextOptions options) : base(options)
        {
           Database.EnsureCreated();
        }
    }
}
