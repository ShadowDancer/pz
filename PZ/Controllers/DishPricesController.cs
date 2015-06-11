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
    public class DishPricesController : Controller
    {
        private PZEntities db = new PZEntities();

        // GET: DishPrices
        public async Task<ActionResult> Index()
        {
            var dishPrices = db.DishPrices.Include(d => d.Dish);
            return View(await dishPrices.ToListAsync());
        }

        // GET: DishPrices/Details/5
        public async Task<ActionResult> Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishPrices dishPrices = await db.DishPrices.FindAsync(id);
            if (dishPrices == null)
            {
                return HttpNotFound();
            }
            return View(dishPrices);
        }

        // GET: DishPrices/Create
        public ActionResult Create()
        {
            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description");
            return View();
        }

        // POST: DishPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Price,DateFrom,DateTo,DishID")] DishPrices dishPrices)
        {
            if (ModelState.IsValid)
            {
                db.DishPrices.Add(dishPrices);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description", dishPrices.DishID);
            return View(dishPrices);
        }

        // GET: DishPrices/Edit/5
        public async Task<ActionResult> Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishPrices dishPrices = await db.DishPrices.FindAsync(id);
            if (dishPrices == null)
            {
                return HttpNotFound();
            }
            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description", dishPrices.DishID);
            return View(dishPrices);
        }

        // POST: DishPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Price,DateFrom,DateTo,DishID")] DishPrices dishPrices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dishPrices).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description", dishPrices.DishID);
            return View(dishPrices);
        }

        // GET: DishPrices/Delete/5
        public async Task<ActionResult> Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishPrices dishPrices = await db.DishPrices.FindAsync(id);
            if (dishPrices == null)
            {
                return HttpNotFound();
            }
            return View(dishPrices);
        }

        // POST: DishPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(DateTime id)
        {
            DishPrices dishPrices = await db.DishPrices.FindAsync(id);
            db.DishPrices.Remove(dishPrices);
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
