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
    public class WaiterController : BaseAdminController
    {
        private PZEntities db = new PZEntities();

        // GET: /Waiter/
        public ActionResult Index()
        {
            return View(db.Waiter.ToList());
        }

        // GET: /Waiter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waiter waiter = db.Waiter.Find(id);
            if (waiter == null)
            {
                return HttpNotFound();
            }
            return View(waiter);
        }

        // GET: /Waiter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Waiter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Surname,Adress,City,Email,Phone,Reputation,Salary,Sex")] Waiter waiter)
        {
            if (ModelState.IsValid)
            {
                db.Waiter.Add(waiter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(waiter);
        }

        // GET: /Waiter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waiter waiter = db.Waiter.Find(id);
            if (waiter == null)
            {
                return HttpNotFound();
            }
            return View(waiter);
        }

        // POST: /Waiter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Surname,Adress,City,Email,Phone,Reputation,Salary,Sex")] Waiter waiter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waiter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(waiter);
        }

        // GET: /Waiter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waiter waiter = db.Waiter.Find(id);
            if (waiter == null)
            {
                return HttpNotFound();
            }
            return View(waiter);
        }

        // POST: /Waiter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Waiter waiter = db.Waiter.Find(id);
            db.Waiter.Remove(waiter);
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
