using System;
using System.Collections.Generic;
using System.Linq;
using PZ.Models;

namespace PZ.Controllers.Api.DTO
{
	public class OrderBundleDto
	{
		public OrderBundleDto(UserViewModel user)
		{
			using(var db = new PZEntities())
			{
				var pzUser = db.User.FirstOrDefault(n => n.Email == user.UserName);


				Orders = db.Order.Where(n => n.UserID == pzUser.ID).Select(n => new OrderDTO()
				{
					SubOrders = n.SubOrder.Select(m => new SubOrderDTO()
					{
						Dish = new DishViewModel()
						{
							ID =  m.DishID,
							Name = m.Dish.NameID,
							Price = m.Dish.GetPrice(),
						},
						Quantity = m.Quantity,
					}).ToList(),
				}).ToList();


				foreach (var order in Orders)
				{
					foreach (var suborder in order.SubOrders)
					{
							suborder.Dish.Price = db.Dish.Where(n => n.ID == suborder.Dish.ID).Single().GetPrice();
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