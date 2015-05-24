using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Controllers.Api.DTO
{
	public class PostRequestDTO
	{
		/// <summary>
		/// Email
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Possible actions:
		/// getCart - get shopping cart contents
		/// reserveTable
		/// addToCart
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// Add di
		/// </summary>
		public string Data { get; set; }


		public List<int> CartItems { get; set; }
		public List<int> CartAmount { get; set; }

		// reservation
		public DateTime ReservationDate { get; set; }
		public int? ReservationHour { get; set; }
		public int? ReservationLength { get; set; }
		public int? Table { get; set; }
	}
}