using System;
using System.Net;

namespace Javelin.Api.Rest
{
	public class RestReaderException : Exception
	{
		public RestReaderException(string message)
			: base(message)
		{
		}

		public RestReaderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public HttpStatusCode StatusCode { get; set; }
		public string StatusDescription { get; set; }
	}
}