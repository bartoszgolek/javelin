namespace Javelin.Api.Rest
{
	public interface IWebClientSerializerFactory
	{
		IWebClientSerializer GetSerializer(string contentType);
	}
}