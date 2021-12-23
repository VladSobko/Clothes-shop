using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using shop_proj.Models;
using shop_proj.VModels;

namespace shop_proj.Controllers
{
    public class UserController : Controller
    {
        private UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        private Models.AppContext db;
        public UserController(Models.AppContext context, UserManager<User> userManager)
        
{
                _userManager = userManager;

                db = context;
        }
       

        public async Task<IActionResult> Ordert()
        {
            ViewData["Message"] = "Hello world!";
            List<Sex> sex = await db.Sexs.Include(u => u.Categories).ThenInclude(p => p.Modells).ToListAsync();
            return View(sex);
        }
        public async Task<IActionResult> Tovarind(int? id)
        {
            SelectList kinds = new SelectList(db.Kinds.Where(c => c.TovarId == id), "Id", "Colour");
            ViewBag.Kinds = kinds;

                  
        return View(await db.Tovars.Include(u => u.Kinds).ThenInclude(p => p.Images).Include(u => u.Kinds).ThenInclude(p => p.Sizes).Include(k=>k.Komentars).ThenInclude(l=>l.Answers).FirstOrDefaultAsync(p => p.Id == id)) ;

        }

        public async Task<IActionResult> Index(int page=1)
        {
            
            List<Sex> sex = await db.Sexs.Include(u => u.Categories).ThenInclude(p => p.Modells).ToListAsync();
           ViewBag.Genders = sex;
            int pageSize = 10;   // количество элементов на странице

            IQueryable<Tovar> source = db.Tovars.Include(u => u.Kinds).ThenInclude(p => p.Images).Include(o=>o.Modell).ThenInclude(j=>j.Category);
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageModel pageViewModel = new PageModel(count, page, pageSize);
            IndexModel viewModel = new IndexModel
            {
                PageModel = pageViewModel,
                Tovars = items
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string Search)
        {
            List<Sex> sex = await db.Sexs.Include(u => u.Categories).ThenInclude(p => p.Modells).ToListAsync();
            ViewBag.Genders = sex;
            
            if (!String.IsNullOrEmpty(Search))
            {
             List<Tovar>   tovars = await db.Tovars.Include(u => u.Kinds).ThenInclude(p => p.Images).Where(s => s.Modell.Name.Contains(Search) || s.Modell.Category.Name.Contains(Search) || s.Material.Contains(Search) || s.Brend.Contains(Search)).ToListAsync();
                IQueryable<Tovar> source = db.Tovars.Include(u => u.Kinds).ThenInclude(p => p.Images).Where(s => s.Modell.Name.Contains(Search) || s.Modell.Category.Name.Contains(Search) || s.Material.Contains(Search) || s.Brend.Contains(Search));
                var count = await source.CountAsync();
                PageModel pageViewModel = new PageModel(count, 1, 10);
                IndexModel viewModel = new IndexModel
                {
                    PageModel = pageViewModel,
                    Tovars = tovars
                };
                return View(viewModel);
            }
            else
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult GetImages(int id)
        {
            return PartialView(db.Images.Where(c => c.KindId == id).ToList());
        }
        public ActionResult GetSizes(int id)
        {
            return PartialView(db.Sizes.Where(c => c.KindId == id).ToList());
        }
        [Authorize(Roles = "user")]
        public async Task<object> Addkorzunaitem(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var id1 = _userManager.GetUserId(User);
            Size size = db.Sizes.FirstOrDefault(p => p.Id == id);
            Korzyna korzyna = await db.Korzuna.FirstOrDefaultAsync(p => p.UserId == id1 );
            if (korzyna==null) 
            { korzyna = new Korzyna { User = user }; 
                db.Korzuna.Add(korzyna); }
            korzyna = await db.Korzuna.FirstOrDefaultAsync(p => p.UserId == id1);
            Korzynaitem item = new Korzynaitem { Size = size, Korzyna = korzyna, Count = 1 };
            size.Count=size.Count-1;
             
            if (size.Count == 0)
                db.Sizes.Remove(size);
            else db.Sizes.Update(size);
            db.Korzynaitems.Add(item);
           
            await db.SaveChangesAsync();
            return korzyna.Id;
        }
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Korzyna()
        {
            List<String> modell = new List<string>();
            modell.Add("Кур'єр");
            modell.Add("На відділення нової пошти");
            modell.Add("На відділення укрпошти");
            ViewBag.Delivery = modell;
            var id = _userManager.GetUserId(User);
            KorzynaOrder k = new KorzynaOrder();
            k.korzyna = await db.Korzuna.Include(u => u.Items).ThenInclude(i => i.Size).ThenInclude(p => p.Kind).ThenInclude(k => k.Tovar).Include(u => u.Items).ThenInclude(i => i.Size).ThenInclude(p => p.Kind).ThenInclude(k => k.Images).FirstOrDefaultAsync(p => p.UserId == id);
            return View(k);

        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(KorzynaOrder korzynaOrder)
        {
            var user = await _userManager.GetUserAsync(User);
            Order order = new Order();
            order = korzynaOrder.order;
            order.User = user;
            var id = _userManager.GetUserId(User);
            Korzyna k = new Korzyna();
            k = await db.Korzuna.Include(u => u.Items).ThenInclude(i => i.Size).ThenInclude(p => p.Kind).ThenInclude(k => k.Tovar).Include(u => u.Items).ThenInclude(i => i.Size).ThenInclude(p => p.Kind).ThenInclude(k => k.Images).FirstOrDefaultAsync(p => p.UserId == id);



            String[] strlist = korzynaOrder.items.Split(",");
            
              db.Orders.Add(order);
            foreach (String s in strlist)
            {
                Size size = await db.Sizes.FirstOrDefaultAsync(p => p.Id == Int32.Parse(s));
                Orderitem item = new Orderitem { Order = order, Size = size };
                foreach(var item1 in k.Items)
                {
                    if (item1.SizeId == Int32.Parse(s))
                    {
                        db.Korzynaitems.Remove(item1);
                        break;
                    }
                }
                
                db.Orderitems.Add(item);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Bill",  new { id = order.Id });
        }
        public async Task<IActionResult> Bill(int id)
        {
            Order order = await db.Orders.Include(u => u.Items).ThenInclude(o => o.Size).ThenInclude(p => p.Kind).ThenInclude(t => t.Images).Include(l => l.Items).ThenInclude(e => e.Size).ThenInclude(b => b.Kind).ThenInclude(c => c.Tovar).Where(c => c.Id == id).FirstAsync();
            ViewBag.Date = order.datezam.AddDays(2);
                return View(order);
        }
        public IActionResult Check(String[] s)
        {
            ViewBag.S = s;
            return View();
        }
        
        public async Task<ActionResult> GetPrise(string[] mas)
        {
            double price = 0;
          
                foreach (String str in mas)
                {
                    Size size = await db.Sizes.Include(u => u.Kind).ThenInclude(p => p.Tovar).FirstOrDefaultAsync(p => p.Id == Int32.Parse(str));
                    price += size.Kind.Tovar.Price;

                }
            
           
ViewBag.Price = price.ToString();

            return PartialView();
        }
       

        public ActionResult GetFilters(int id)
        {
            return PartialView(db.Images.Where(c => c.KindId == id).ToList());
        }

        public ActionResult GetTovars(int id)
        {
            List<Category> categories = db.Categories.Include(p => p.Modells).ThenInclude(c => c.Tovars).ThenInclude(u => u.Kinds).ThenInclude(o => o.Images).Where(c => c.SexId == id).ToList();
            List<Tovar> tovars = new List<Tovar>();
            foreach(var category in categories)
            {
                foreach(var modell in category.Modells)
                {
                    foreach (var tovar in modell.Tovars)
                    {
                        tovars.Add(tovar);

                    }

                }
                
            }
            return PartialView(tovars);
        }
        public ActionResult GetTovarsCat(int id)
        {
            List<Modell> modells = db.Modells.Include(c => c.Tovars).ThenInclude(u => u.Kinds).ThenInclude(o => o.Images).Where(c => c.CategoryId == id).ToList();
            List<Tovar> tovars = new List<Tovar>();
           
                foreach (var modell in modells)
                {
                    foreach (var tovar in modell.Tovars)
                    {
                        tovars.Add(tovar);

                    }

                }

            
            return PartialView("GetTovars", tovars);
        }
        public ActionResult GetTovarsMod(int id)
        {
             List<Tovar> tovars = db.Tovars.Include(u => u.Kinds).ThenInclude(o => o.Images).Where(c => c.ModellId == id).ToList();
            return PartialView("GetTovars", tovars);
        }
        [HttpPost]
        public IActionResult AddAnswer([FromBody] Answer data)
        {
            var user = _userManager.GetUserId(User);
              Komentar komentar = db.Komentars.FirstOrDefault(p => p.Id == data.KomentarId);
               Answer answer = new Answer { UserId = user, Komentar = komentar, Text = data.Text, date = DateTime.Now };
                db.Answers.Add(answer);
              db.SaveChanges();
            Tovar tovar = db.Tovars.Include(u => u.Kinds).ThenInclude(p => p.Images).Include(k => k.Komentars).ThenInclude(l => l.Answers).FirstOrDefault(p => p.Id == komentar.TovarId) ;
            Komentar komentar1 = db.Komentars.Include(p=>p.Answers).FirstOrDefault(p => p.Id == data.KomentarId);
            return PartialView("GetAnswers", komentar1);
        }

        [HttpPost]
        public IActionResult AddKomentar([FromBody] Komentar data)
        {
            var user =  _userManager.GetUserId(User);
            Tovar tovar = db.Tovars.FirstOrDefault(p => p.Id == data.TovarId);
            Komentar komentar = new Komentar { UserId = user, Tovar = tovar, Text = data.Text, date = DateTime.Now };
           db.Komentars.Add(komentar);
            db.SaveChanges();
            Tovar tovar1 = db.Tovars.Include(u => u.Kinds).ThenInclude(p => p.Images).Include(k => k.Komentars).ThenInclude(l => l.Answers).FirstOrDefault(p => p.Id == data.TovarId);

            return PartialView("GetKoments", tovar1);
        }
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Orders()
        {
            var id = _userManager.GetUserId(User);
            return View(await db.Orders.Include(u => u.Items).ThenInclude(o=>o.Size).ThenInclude(p=>p.Kind).ThenInclude(t=>t.Images).Include(l=>l.Items).ThenInclude(e=>e.Size).ThenInclude(b=>b.Kind).ThenInclude(c=>c.Tovar).Where(c => c.UserId == id).ToListAsync());
        }


    }
}
