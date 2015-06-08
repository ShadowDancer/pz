using System;
using System.Collections.Generic;
using System.Linq;
using PZ.Models;

namespace PZ.Controllers.Api.DTO
{
	public class OrderBundleDto
	{
		public OrderBundleDto()
		{
			using(var db = new PZEntities())
			{
				Orders = db.Order.Select(n => new OrderDTO()
				{
					SubOrders = n.SubOrder.Select(m => new SubOrderDTO()
					{
						Dish = new DishViewModel()
						{
							ID =  m.DishID,
							Name = m.Dish.NameID,
							Price = 0,
						},
						Quantity = m.Quantity,
					}).ToList(),
				}).ToList();


				var prices = db.DishPrices.Where(n => n.DateTo == null || n.DateTo > DateTime.Now).OrderBy(k => k.DateFrom).Take(1).Select(m => new
				{
					m.DishID,
					m.Price
				}).ToList();

				foreach (var order in Orders)
				{
					foreach (var suborder in order.SubOrders)
					{
						var price = prices.FirstOrDefault(n => n.DishID == suborder.Dish.ID);
						if (price != null)
						{
							suborder.Dish.Price = price.Price;
						}
					}
				}
			}
		}

		public List<OrderDTO> Orders { get; set; }
	}

	public class OrderDTO
	{
		public DateTime date { get; set; }
		public int state { get; set; }
		public List<SubOrderDTO> SubOrders { get; set; }
	}

	public class SubOrderDTO
	{
		public int Quantity { get; set; }
		public DishViewModel Dish { get; set; }
	}
}