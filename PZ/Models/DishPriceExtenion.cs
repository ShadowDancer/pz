using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
	public static class DishPriceExtenion
	{
		public static decimal GetPrice(this Dish d)
		{
			var price =
				d.DishPrices.Where(n => n.DateTo == null || n.DateTo > DateTime.Now)
					.OrderBy(k => k.DateFrom)
					.Take(1)
					.FirstOrDefault();
			if (price != null)
				return price.Price;
			else
			{
				return 0;
			}
		}

		public static string GetStatusName(this Order o, bool waiter = false)
		{
			if (!waiter)
			{
				switch (o.State)
				{
					case OrderState.open:
						return "W realizacji";
					case OrderState.wip:
						return "W realizacji";
					case OrderState.realised:
						return "Oczekuje na zapłatę";
					case OrderState.paymentRequested:
						return "Zażądano rachunku";
					case OrderState.paid:
						return "Zakończone";

				}
			}
			else
			{
				switch (o.State)
				{
					case OrderState.open:
						return "Oczekuje na zatwierdzenie";
					case OrderState.wip:
						return "Oczekuje na realizację";
					case OrderState.realised:
						return "Oczekuje na zapłatę";
					case OrderState.paymentRequested:
						return "Zażądano zapłaty";
					case OrderState.paid:
						return "Zakończone";

				}
			}

			return o.State.ToString();

		}
	}
}