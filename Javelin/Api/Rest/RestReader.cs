namespace Javelin.Api.Rest
{
	public class RestReader : IRestReader
	{
		public RestReader(IWebClientSerializerFactory webClientSerializerFactory)
		{
			this.webClientSerializerFactory = webClientSerializerFactory;
		}

		public RestReaderResponse<TResponse> GetResponse<TResponse>(string uri, string contentType)
		{
			var preparer = new RequestPreparer(contentType);
			return RestRequestBuilder.Request<TResponse>(webClientSerializerFactory, uri, preparer);
		}

		public RestReaderResponse<TResponse> PostRequest<TResponse, TRequest>(string uri, string contentType, TRequest requestData)
		{
			var preparer = new RequestPreparer(
				stream => webClientSerializerFactory
					.GetSerializer(contentType)
					.ToStream(stream, requestData),
					contentType
			);
			return RestRequestBuilder.Request<TResponse>(webClientSerializerFactory, uri, preparer);
		}

		private readonly IWebClientSerializerFactory webClientSerializerFactory;
	}
}