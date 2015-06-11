using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PZ.Models;

namespace PZ.Controllers
{
    [Authorize(Roles = "admin")]
    public class DishController : Controller
    {
        private PZEntities db = new PZEntities();

        // GET: Dish
        public async Task<ActionResult> Index()
        {
            var dish = db.Dish.Include(d => d.MenuSubcategory1);
            return View(await dish.ToListAsync());
        }

        // GET: Dish/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = await db.Dish.FindAsync(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // GET: Dish/Create
        public ActionResult Create()
        {
            ViewBag.SubcategoryID = new SelectList(db.MenuSubcategory, "ID", "Subcategory");
            return View();
        }

        // POST: Dish/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Description,Comment,Rating,NameID,ImageUrl,SubcategoryID")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Dish.Add(dish);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.SubcategoryID = new SelectList(db.MenuSubcategory, "ID", "Subcategory", dish.SubcategoryID);
            return View(dish);
        }

        // GET: Dish/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = await db.Dish.FindAsync(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubcategoryID = new SelectList(db.MenuSubcategory, "ID", "Subcategory", dish.SubcategoryID);
            return View(dish);
        }

        // POST: Dish/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Description,Comment,Rating,NameID,ImageUrl,SubcategoryID")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dish).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubcategoryID = new SelectList(db.MenuSubcategory, "ID", "Subcategory", dish.SubcategoryID);
            return View(dish);
        }

        // GET: Dish/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = await db.Dish.FindAsync(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dish dish = await db.Dish.FindAsync(id);
            db.Dish.Remove(dish);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
