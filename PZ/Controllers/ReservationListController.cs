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
    public class ReservationListController : Controller
    {
        private PZEntities db = new PZEntities();

		
		public new ActionResult View()
		{
			var requestDate = DateTime.Now;
			using(var sr = new System.IO.StreamReader(Request.InputStream))
			{
				string inputDate = null;
				inputDate = sr.ReadToEnd();
				var post = HttpUtility.ParseQueryString(inputDate);

				if(!string.IsNullOrEmpty(post["value"]))
				{
					requestDate = DateTime.Parse(post["value"]);
				}
			}


			return View(new ReservationListViewModel(requestDate));
		}

        // GET: /ReservationList/
        public ActionResult Index()
        {
            var reservation_list = db.Reservation_List.Include(r => r.Table).Include(r => r.User);
            return View(reservation_list.ToList());
        }

        // GET: /ReservationList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservation_list = db.Reservation_List.Find(id);
            if (reservation_list == null)
            {
                return HttpNotFound();
            }
            return View(reservation_list);
        }

        // GET: /ReservationList/Create
        public ActionResult Create()
        {
            ViewBag.TableID = new SelectList(db.Table, "ID", "Comment");
            ViewBag.UserID = new SelectList(db.User, "ID", "City");
            return View();
        }

        // POST: /ReservationList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,From,To,UserID,TableID")] Reservation_List reservation_list)
        {
            if (ModelState.IsValid)
            {
                db.Reservation_List.Add(reservation_list);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TableID = new SelectList(db.Table, "ID", "Comment", reservation_list.TableID);
            ViewBag.UserID = new SelectList(db.User, "ID", "City", reservation_list.UserID);
            return View(reservation_list);
        }

        // GET: /ReservationList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservation_list = db.Reservation_List.Find(id);
            if (reservation_list == null)
            {
                return HttpNotFound();
            }
            ViewBag.TableID = new SelectList(db.Table, "ID", "Comment", reservation_list.TableID);
            ViewBag.UserID = new SelectList(db.User, "ID", "City", reservation_list.UserID);
            return View(reservation_list);
        }

        // POST: /ReservationList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,From,To,UserID,TableID")] Reservation_List reservation_list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation_list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TableID = new SelectList(db.Table, "ID", "Comment", reservation_list.TableID);
            ViewBag.UserID = new SelectList(db.User, "ID", "City", reservation_list.UserID);
            return View(reservation_list);
        }

        // GET: /ReservationList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reservation_list = db.Reservation_List.Find(id);
            if (reservation_list == null)
            {
                return HttpNotFound();
            }
            return View(reservation_list);
        }

        // POST: /ReservationList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reservation_list = db.Reservation_List.Find(id);
            db.Reservation_List.Remove(reservation_list);
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
