using System;
using System.Net;

namespace Javelin.Api.Rest
{
	public class RestReaderResponse<T>
	{
		public RestReaderResponse(Uri responseUri, HttpStatusCode statusCode, T content)
		{
			ResponseUri = responseUri;
			StatusCode = statusCode;
			Content = content;
		}

		public Uri ResponseUri { get; private set; }

		public HttpStatusCode StatusCode { get; private set; }

		public T Content { get; private set; }
	}
}