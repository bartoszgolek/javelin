namespace Javelin.Api.Rest
{
	public interface IRestReader
	{
		RestReaderResponse<TResponse> GetResponse<TResponse>(string uri, string contentType);

		RestReaderResponse<TResponse> PostRequest<TResponse, TRequest>(string uri, string contentType, TRequest requestData);
	}
}