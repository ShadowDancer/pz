using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
    public class OrderBundleViewModel
    {
        public OrderBundleViewModel(UserViewModel user)
        {
            using(var db = new PZEntities())
            {
                Orders = db.Order.Where(n => n.UserID == user.ID).OrderBy(n => n.IssueDate).Select(n => new OrderViewModel(n, db)).ToList();
            }
        }

        public DateTime OrderDate { get; set; }

        public List<OrderViewModel> Orders;
    }

    public class OrderViewModel
    {
        public OrderViewModel(Order order, PZEntities db)
        {
            Suborders = db.SubOrder.Where(n => n.OrderID == order.ID).Select(n => new SuborderViewModel(n, db)).ToList();
        }

        public DateTime SuborderDate { get; set; }

        public List<SuborderViewModel> Suborders;
    }

    public class SuborderViewModel
    {
        public SuborderViewModel(SubOrder suborder, PZEntities db)
        {
        }

        public string Name { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }

    }
}