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
							Price = 0
						},
						Quantity = m.Quantity,
					}).ToList(),
				}).ToList();
			}
		}

		public List<OrderDTO> Orders { get; set; }
	}

	public class OrderDTO
	{
		public int state;
		public List<SubOrderDTO> SubOrders { get; set; }
	}

	public class SubOrderDTO
	{
		public int Quantity { get; set; }
		public DishViewModel Dish { get; set; }
	}
}