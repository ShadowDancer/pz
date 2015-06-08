using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace PZ
{
	public static class OrderState
	{
		public const int open = 0;
		public const int realised = 10;
		public const int paymentRequested = 20;
		public const int paid = 30;
	}
}