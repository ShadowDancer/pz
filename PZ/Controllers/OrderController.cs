﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using PZ.Models;

namespace PZ.Controllers
{
	public class OrderController : Controller
	{
		private PZEntities db = new PZEntities();

		// GET: /Order/
		public ActionResult Index()
		{
			var order = db.Order.Include(o => o.Table).Include(o => o.User).Include(o => o.Waiter);
			return View(order.ToList());
		}

		// GET: /Order/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var order = db.Order.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// GET: /Order/Create
		public ActionResult Create()
		{
			ViewBag.TableID = new SelectList(db.Table, "ID", "Comment");
			ViewBag.UserID = new SelectList(db.User, "ID", "City");
			ViewBag.WaiterID = new SelectList(db.Waiter, "ID", "Name");
			return View();
		}

		public ActionResult OrderList()
		{

			List<Order> orderList = db.Order.Where(n => n.User.Email == User.Identity.Name).ToList();
			return View(orderList);
		}

		public ActionResult CheckOrder()
		{
			CheckOrderViewModel vm = new CheckOrderViewModel();
			List<Order> orderList = db.Order.Where(n => n.User.Email == User.Identity.Name && n.State < OrderState.paid).ToList();
			vm.OpenOrders = orderList.Where(n => n.State == OrderState.open).ToList();
			vm.RealisedOrders = orderList.Where(n => n.State == OrderState.realised).ToList();
			vm.WaitingForPayments = orderList.Where(n => n.State == OrderState.paymentRequested).ToList();
			return View(orderList);
		}

		public ActionResult ShoppingCart()
		{
			ShoppingCartModel model = new ShoppingCartModel();
			model.ShoppingCart = db.ShoppingCart.Where(n => n.User.Email == User.Identity.Name).ToList();
			return View(model);
		}


		public ActionResult OrderWaiter()
		{

			List<Order> orderList = db.Order.Where(n => n.State < OrderState.paid).ToList();
			return View(orderList);
		}

		public ActionResult Suborder()
		{
			return View();
		}


		// POST: /Order/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ID,UserID,TableID,WaiterID,Comment,IssueDate,State,Value")] Order order)
		{
			if (ModelState.IsValid)
			{
				db.Order.Add(order);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.TableID = new SelectList(db.Table, "ID", "Comment", order.TableID);
			ViewBag.UserID = new SelectList(db.User, "ID", "City", order.UserID);
			ViewBag.WaiterID = new SelectList(db.Waiter, "ID", "Name", order.WaiterID);
			return View(order);
		}

		// GET: /Order/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var order = db.Order.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			ViewBag.TableID = new SelectList(db.Table, "ID", "Comment", order.TableID);
			ViewBag.UserID = new SelectList(db.User, "ID", "City", order.UserID);
			ViewBag.WaiterID = new SelectList(db.Waiter, "ID", "Name", order.WaiterID);
			return View(order);
		}

		// POST: /Order/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,UserID,TableID,WaiterID,Comment,IssueDate,State,Value")] Order order)
		{
			if (ModelState.IsValid)
			{
				db.Entry(order).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.TableID = new SelectList(db.Table, "ID", "Comment", order.TableID);
			ViewBag.UserID = new SelectList(db.User, "ID", "City", order.UserID);
			ViewBag.WaiterID = new SelectList(db.Waiter, "ID", "Name", order.WaiterID);
			return View(order);
		}

		// GET: /Order/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var order = db.Order.Find(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		// POST: /Order/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var order = db.Order.Find(id);
			db.Order.Remove(order);
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

		public ActionResult Inc(int id)
		{
			var item = db.ShoppingCart.Where(n => n.ID == id).Single();
			item.Quantity++;

			db.SaveChanges();

			return RedirectToAction("ShoppingCart");
		}

		public ActionResult Dec(int id)
		{

			var item = db.ShoppingCart.Where(n => n.ID == id).Single();
			item.Quantity--;
			if (item.Quantity == 0)
			{
				db.ShoppingCart.Remove(item);
			}

			db.SaveChanges();
			return RedirectToAction("ShoppingCart");
		}

		public ActionResult Del(int id)
		{
			var item = db.ShoppingCart.Where(n => n.ID == id).Single();
			db.ShoppingCart.Remove(item);
			db.SaveChanges();
			return RedirectToAction("ShoppingCart");
		}

		public ActionResult OrderCreate()
		{
			var PZUser = db.User.Where(n => n.Email == User.Identity.Name).FirstOrDefault();
			var newOrder = new Order() { IssueDate = DateTime.Now, State = 1, TableID = 1, UserID = PZUser.ID };
			db.Order.Add(newOrder);
			db.SaveChanges();

			var shoppingCartItems = db.ShoppingCart.Where(n => n.UserID == PZUser.ID);
			foreach (var shoppingCartItem in (shoppingCartItems))
			{
				var subOrder = new SubOrder();
				subOrder.DishID = shoppingCartItem.DishID;
				subOrder.Quantity = shoppingCartItem.Quantity;
				subOrder.OrderID = newOrder.ID;
				db.SubOrder.Add(subOrder);
			}
			db.ShoppingCart.RemoveRange(shoppingCartItems);

			db.SaveChanges();
			return RedirectToAction("OrderList");
		}
	}
}
