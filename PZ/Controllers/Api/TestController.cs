using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;

namespace PZ.Controllers.Api
{
	public class TestController : ApiController
	{
		public string Post([FromBody] string value)
		{
			return value;
		}
	}
}