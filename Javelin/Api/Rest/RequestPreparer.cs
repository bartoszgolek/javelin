using System;
using System.IO;
using System.Net;

namespace Javelin.Api.Rest
{
	public class RequestPreparer
	{
		private readonly Action<Stream> writeToStream;
		private readonly string contentType;

		public RequestPreparer(string contentType)
		{
			this.contentType = contentType;
		}

		public RequestPreparer(Action<Stream> writeToStream, string contentType)
			: this(contentType)
		{
			this.writeToStream = writeToStream;
		}

		public WebRequest PrepareRequest(string uri)
		{
			var request = CreateRequest(uri);
			if (writeToStream != null)
				PreparePostRequest(request, writeToStream);
			return request;
		}

		private WebRequest CreateRequest(string uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Timeout = GetTimeout();
			request.Accept = GetResponseType();
			return request;
		}

		private static int GetTimeout()
		{
			return (int)TimeSpan.FromMinutes(2).TotalMilliseconds;
		}

		private void PreparePostRequest(WebRequest request, Action<Stream> writeToStream)
		{
			PreparePostMethod(request);
			PreparePostData(request, writeToStream);
		}

		private void PreparePostMethod(WebRequest request)
		{
			request.ContentType = GetContentType();
			request.Method = "POST";
		}

		private static void PreparePostData(WebRequest request, Action<Stream> writeToStream)
		{
			var requestStream = request.GetRequestStream();
			writeToStream(requestStream);
			requestStream.Close();
		}

		private string GetResponseType()
		{
			return contentType;
		}

		private string GetContentType()
		{
			return contentType;
		}
	}
}