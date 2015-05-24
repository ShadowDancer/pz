﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
	public class ShoppingCartViewModel
	{
		public ShoppingCartViewModel(string userEmail)
		{
			using(PZEntities db = new PZEntities())
			{
				List<ShoppingCart> list = db.ShoppingCart.Where(n => n.User.Email == userEmail).ToList();
				Quantity = new List<int>();
				Dishes = new List<DishViewModel>();

				foreach (var item in list)
				{
					Quantity.Add(item.Quantity);
					Dishes.Add(new DishViewModel() { ID = item.Dish.ID, Name = item.Dish.NameID, Price = 0 });
				}
			}
		}

		List<DishViewModel> Dishes { get; set; }
		List<int> Quantity { get; set; }
	}
}