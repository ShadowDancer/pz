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
    public class MenuController : Controller
    {
        private PZEntities db = new PZEntities();

        public ActionResult Menu()
        {
            return View(new MenuBundleViewModel());
        }

        // GET: /Default1/
        public ActionResult Index()
        {
            return View(db.Menu.ToList());
        }

        // GET: /Default1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: /Default1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Default1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Category,Subcategory")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: /Default1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpPost]
        public ActionResult _EditMenuDescription(string pk, string value)
        {
            using (PZEntities db = new PZEntities())
            {
                int id = 0;
                if (!string.IsNullOrEmpty(pk))
                {
                    id = int.Parse(pk);
                }
                var menu = db.Menu.FirstOrDefault(n => n.ID == id);
                if (menu == null)
                {
                    menu = new Menu();
                    db.Menu.Add(menu);
                }

                menu.Category = value;
                db.SaveChanges();
            }


            return null;
        }

        [HttpPost]
        public ActionResult _EditSubmenuDescription(string pk, string value)
        {
            using (PZEntities db = new PZEntities())
            {
                int id = 0;
                if (!string.IsNullOrEmpty(pk))
                {
                    id = int.Parse(pk);
                }
                var subMenu = db.MenuSubcategory.FirstOrDefault(n => n.ID == id);
                if (subMenu == null)
                {
                    subMenu = new MenuSubcategory();
                    db.MenuSubcategory.Add(subMenu);
                }

                subMenu.Subcategory = value;
                db.SaveChanges();
            }


            return null;
        }
        // POST: /Default1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Category,Subcategory")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: /Default1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: /Default1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menu.Find(id);
            db.Menu.Remove(menu);
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
