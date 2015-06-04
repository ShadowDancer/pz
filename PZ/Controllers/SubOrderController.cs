using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PZ.Models;

namespace PZ.Controllers
{
    public class SubOrderController : Controller
    {
        private PZEntities db = new PZEntities();

        // GET: /SubOrder/
        public ActionResult Index()
        {
            var suborder = db.SubOrder.Include(s => s.Dish).Include(s => s.Order);
            return View(suborder.ToList());
        }

        // GET: /SubOrder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var suborder = db.SubOrder.Find(id);
            if (suborder == null)
            {
                return HttpNotFound();
            }
            return View(suborder);
        }

        // GET: /SubOrder/Create
        public ActionResult Create()
        {
            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description");
            ViewBag.OrderID = new SelectList(db.Order, "ID", "Comment");
            return View();
        }

        // POST: /SubOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,OrderID,DishID,Quantity")] SubOrder suborder)
        {
            if (ModelState.IsValid)
            {
                db.SubOrder.Add(suborder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description", suborder.DishID);
            ViewBag.OrderID = new SelectList(db.Order, "ID", "Comment", suborder.OrderID);
            return View(suborder);
        }

        // GET: /SubOrder/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var suborder = db.SubOrder.Find(id);
            if (suborder == null)
            {
                return HttpNotFound();
            }
            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description", suborder.DishID);
            ViewBag.OrderID = new SelectList(db.Order, "ID", "Comment", suborder.OrderID);
            return View(suborder);
        }

        // POST: /SubOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,OrderID,DishID,Quantity")] SubOrder suborder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suborder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DishID = new SelectList(db.Dish, "ID", "Description", suborder.DishID);
            ViewBag.OrderID = new SelectList(db.Order, "ID", "Comment", suborder.OrderID);
            return View(suborder);
        }

        // GET: /SubOrder/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var suborder = db.SubOrder.Find(id);
            if (suborder == null)
            {
                return HttpNotFound();
            }
            return View(suborder);
        }

        // POST: /SubOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var suborder = db.SubOrder.Find(id);
            db.SubOrder.Remove(suborder);
            db.SaveChanges();
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
