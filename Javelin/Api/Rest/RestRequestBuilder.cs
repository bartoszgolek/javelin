using System.IO;
using System.Net;

namespace Javelin.Api.Rest
{
	public static class RestRequestBuilder
	{
		public static RestReaderResponse<T> Request<T>(IWebClientSerializerFactory webClientSerializerFactory, string uri, RequestPreparer requestPreparer)
		{
			var request = requestPreparer.PrepareRequest(uri);
			return GetResponse<T>(webClientSerializerFactory, request);
		}

		private static RestReaderResponse<T> GetResponse<T>(IWebClientSerializerFactory webClientSerializerFactory, WebRequest request)
		{
			try
			{
				using (var response = request.GetResponse())
				{
					var httpWebResponse = response as HttpWebResponse;
					var responseStatus = httpWebResponse != null
						? httpWebResponse.StatusCode
						: 0;

					if (response.ContentLength == 0)
						return new RestReaderResponse<T>(response.ResponseUri, responseStatus, default(T));

					var responseStream = response.GetResponseStream();
					if (responseStream == null)
						return new RestReaderResponse<T>(response.ResponseUri, responseStatus, default(T));

					return new RestReaderResponse<T>(response.ResponseUri, responseStatus, ReadContent<T>(webClientSerializerFactory, response, responseStream));
				}
			}
			catch (WebException webException)
			{
				if (webException.Response == null)
					throw;

				var restReaderException = new RestReaderException(webException.Message, webException);

				var httpWebResponse = webException.Response as HttpWebResponse;
				if (httpWebResponse != null)
				{
					restReaderException.StatusCode = httpWebResponse.StatusCode;
					restReaderException.StatusDescription = httpWebResponse.StatusDescription;
				}

				throw restReaderException;
			}
		}

		private static TResponse ReadContent<TResponse>(
			IWebClientSerializerFactory webClientSerializerFactory,
			WebResponse response,
			Stream responseStream)
		{
			var webClientSerializer = webClientSerializerFactory.GetSerializer(response.ContentType.Split(';')[0]);
			using (var sr = new StreamReader(responseStream))
			{
				string s = sr.ReadToEnd();
				return string.IsNullOrEmpty(s)
					? default(TResponse)
					: webClientSerializer.Deserialize<TResponse>(s);
			}
		}
	}
}