using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
	public class ShoppingCartModel
	{
		public List<ShoppingCart> ShoppingCart { get; set; }
		public decimal ShoppingCartPrice { get; set; }

	}
}