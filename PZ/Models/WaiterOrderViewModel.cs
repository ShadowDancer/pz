using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
    public class WaiterOrderBundleViewModel
    {
		public WaiterOrderBundleViewModel(UserViewModel user)
        {
            using(var db = new PZEntities())
            {
                Orders = db.Order.Where(n => n.UserID == user.ID).OrderBy(n => n.IssueDate).Select(n => new WaiterOrderViewModel(n, db)).ToList();
            }
        }

        public DateTime OrderDate { get; set; }

        public List<WaiterOrderViewModel> Orders;
    }

    public class WaiterOrderViewModel
    {
        public WaiterOrderViewModel(Order order, PZEntities db)
        {
            Suborders = db.SubOrder.Where(n => n.OrderID == order.ID).Select(n => new WaiterSuborderViewModel(n, db)).ToList();
        }

        public DateTime SuborderDate { get; set; }

        public List<WaiterSuborderViewModel> Suborders;
    }

    public class WaiterSuborderViewModel
    {
        public WaiterSuborderViewModel(SubOrder suborder, PZEntities db)
        {
	        Name = suborder.Dish.NameID;
	        Quantity = suborder.Quantity;
	        Price = 0;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

    }
}