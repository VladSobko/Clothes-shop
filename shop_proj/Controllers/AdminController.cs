using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shop_proj.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shop_proj.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private Models.AppContext db;
        IWebHostEnvironment _appEnvironment;
        public AdminController(Models.AppContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Tovars.Include(p=>p.Modell).ThenInclude(c=>c.Category).ToListAsync());
        }
        public async Task<IActionResult> Categories()
        {
            return View(await db.Categories.Include(h=>h.Sex).ToListAsync());
        }
        public async Task<IActionResult> Addcategory()
        {
            List<Sex> sex = await db.Sexs.ToListAsync();
            ViewBag.Sex = sex;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Addcategory(Category user)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);

            if (!Validator.TryValidateObject(user, context, results, true))
            {
                return View();
            }
            else
            {
                db.Categories.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("EditCategory", new { id = user.Id });
            }
           
        }
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id != null)
            {
                Category user = await db.Categories.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(await db.Categories.Include(u => u.Modells).FirstOrDefaultAsync(p => p.Id == id));

            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category user)
        {
            db.Categories.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Addtovar()
        {
            SelectList sex = new SelectList(db.Sexs, "Id", "Name");
            ViewBag.Sex = sex;
            SelectList category = new SelectList(db.Categories.Where(p => p.SexId == 1), "Id", "Name");
            List<Modell> modell = await db.Modells.Where(p => p.CategoryId ==  db.Categories.Where(p => p.SexId == 1).FirstOrDefault().Id).ToListAsync();
            ViewBag.Category = category;
            ViewBag.Modell = modell;
            return View();
        }
        public IActionResult AddModell(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddModell(Modell user)
        {
            Category category = await db.Categories.FirstOrDefaultAsync(p => p.Id == user.CategoryId);
            
           Modell modell= new Modell { Name = user.Name, Category = category };

            db.Modells.Add(modell);
          await db.SaveChangesAsync();
            return RedirectToAction("EditCategory", new { id = category.Id });
        }


        [HttpPost]
        public async Task<IActionResult> Addtovar(Tovar user)
        {
            if (ModelState.IsValid)
            {
                db.Tovars.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Tovarinf", new { id = user.Id });
            }
            else
            {
                SelectList sex = new SelectList(db.Sexs, "Id", "Name");
                ViewBag.Sex = sex;
                SelectList category = new SelectList(db.Categories.Where(p => p.SexId == 1), "Id", "Name");
                List<Modell> modell = await db.Modells.Where(p => p.CategoryId == db.Categories.Where(p => p.SexId == 1).FirstOrDefault().Id).ToListAsync();
                ViewBag.Category = category;
                ViewBag.Modell = modell;
                return View();
            }
           

        }
        public async Task<IActionResult> Tovarinf(int? id)
        {
            if (id != null)
            {
                Tovar user = await db.Tovars.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(await db.Tovars.Include(u => u.Kinds).FirstOrDefaultAsync(p => p.Id == id));

            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Tovarinf(Tovar user)
        {
            db.Tovars.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult AddKind(int? id)
        {
            ViewBag.Id = id;
            ViewBag.sizes = new List<String> { "XS", "S", "M", "L", "XL", "XXL", "XXL" };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Addkind(Kind user, List<IFormFile> uploadedFiles, int xxs, int xs, int s, int m, int l, int xl, int xxl)
        {
            Image img = new Image();
            Tovar tovar = await db.Tovars.FirstOrDefaultAsync(p => p.Id == user.TovarId);
            Size size = new Size();

            Kind kind = new Kind { Colour = user.Colour, Tovar = tovar };

            db.Kinds.Add(kind);
            foreach (var uploadedFile in uploadedFiles)
            {
                if (uploadedFile != null)
                {
                    img.Name = uploadedFile.FileName;
                    string path = "/Images/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    Image file = new Image { Name = uploadedFile.FileName, Path = path, Kind = kind };
                    db.Images.Add(file);

                }
            }
            List<Size> tt = new List<Size> { new Size("xxs", xxs),
            new Size("xs",xs), new Size ("s",s), new Size("m",m), new Size("l",l), new Size("xl",xl), new Size("xxl",xxl) };
          
            foreach (var item in tt)
            {
                if (item.Count != 0)
                {
                    Size file = new Size { Name = item.Name, Count = item.Count, Kind = kind };
                    db.Sizes.Add(file);
                }
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Tovarinf",  new { id=tovar.Id });
        }
        public async Task<IActionResult> Editkind(int? id)
        {
            if (id != null)
            {
                Kind user = await db.Kinds.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Editkind(Kind user)
        {
            
            db.Kinds.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Tovarinf", new { id = user.TovarId });
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Tovar user = await db.Tovars.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Tovar user = await db.Tovars.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Tovars.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        [HttpGet]
        public ActionResult GetCategories(int id)
        {
            SelectList category = new SelectList(db.Categories.Where(p => p.SexId == id), "Id", "Name");
            List<Modell> modell = db.Modells.Where(p => p.CategoryId == db.Categories.Where(p => p.SexId == id).FirstOrDefault().Id).ToList();
            ViewBag.Category = category;
            ViewBag.Modell = modell;
            return PartialView();
        }
        [HttpGet]
        public ActionResult GetModells(int id)
        {
            List<Modell> modell = db.Modells.Where(p => p.CategoryId == id).ToList();
          
            ViewBag.Modell = modell;
            return PartialView();
        }
    }
}
