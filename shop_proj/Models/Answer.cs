using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public Komentar Komentar { get; set; }
        public int KomentarId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime date { get; set; }
    }
}
