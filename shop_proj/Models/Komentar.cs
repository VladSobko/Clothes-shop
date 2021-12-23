using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public Tovar Tovar { get; set; }
        public int TovarId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime date { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
