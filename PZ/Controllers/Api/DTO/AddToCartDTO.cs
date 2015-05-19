using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Controllers.Api.DTO
{
	[Serializable]
	public class AddToCartDTO
	{
		List<string> ID { get; set; }
		List<string> Amount { get; set; }
	}
}