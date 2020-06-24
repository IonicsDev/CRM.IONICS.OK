using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Business.Exceptions
{
	public class AppException : Exception
	{
		public AppException() : base() { }
		public AppException(string message) : base(message) { }
	}
}
