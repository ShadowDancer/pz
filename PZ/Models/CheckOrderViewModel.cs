using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
	public class CheckOrderViewModel
	{
		public CheckOrderViewModel()
		{
			
		}

		public decimal OpenAndRealisedOrdersPrice { get; set; }
		public decimal WaitingForPaymentsPrice { get; set; }
		public List<Order> OpenOrders { get; set; }
		public List<Order> RealisedOrders { get; set; }
		public List<Order> WaitingForPayments { get; set; } 

	}
}