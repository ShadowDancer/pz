using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
	public static class DishPriceExtenion
	{
		public static decimal GetPrice(this Dish  d)
		{
			var price =
				d.DishPrices.Where(n => n.DateTo == null || n.DateTo > DateTime.Now)
					.OrderBy(k => k.DateFrom)
					.Take(1)
					.FirstOrDefault();
			if(price != null)
				return price.Price;
			else
			{
				return 0;
			}
		}
	}
}