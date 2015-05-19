using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PZ.Controllers.Api.DTO
{
	class OperationResultDTO
	{
		public OperationResultDTO(bool Success, string Message)
		{
			this.Success = Success;
			this.Message = Message;
		}

		public bool Success { get; set; }

		public string Message { get; set; }
	}
}