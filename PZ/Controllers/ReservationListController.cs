using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PZ.Models;

namespace PZ.Controllers
{
	public class ReservationListController : Controller
	{
		private PZEntities db = new PZEntities();

		public ActionResult RList()
		{
			var requestDate = DateTime.Now;

			string inputDate = Request.QueryString["value"];

			if (!string.IsNullOrEmpty(inputDate))
			{
				requestDate = DateTime.Parse(inputDate);
			}

			var user = db.User.FirstOrDefault(n => n.Email == User.Identity.Name);
			if (user != null)
				return View(new ReservationListViewModel(requestDate, user.ID));
			else
				return View(new ReservationListViewModel(requestDate, -1));
		}

        // GET: /ReservationList/
        [Authorize(Roles = "admin")]
		public ActionResult Index()
		{
			var reservationList = db.Reservation_List.Include(r => r.Table).Include(r => r.User);
			return View(reservationList.ToList());
		}

        // GET: /ReservationList/Details/5
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
		public ActionResult Create()
		{
			ViewBag.TableID = new SelectList(db.Table, "ID", "Comment");
			ViewBag.UserID = new SelectList(db.User, "ID", "City");
			return RList();
		}

		// POST: /ReservationList/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
		public ActionResult Create([Bind(Include = "ID,From,To,UserID,TableID")] Reservation_List reservation_list)
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
		public ActionResult Edit([Bind(Include = "ID,From,To,UserID,TableID")] Reservation_List reservation_list)
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
