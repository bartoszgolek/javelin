using System.IO;

namespace Javelin.Api.Rest
{
	public interface IWebClientSerializer
	{
		void ToStream<T>(Stream stream, T entity);

		string Serialize<T>(T entity);

		T Deserialize<T>(string entity);

		T Deserialize<T>(Stream entityStream);
	}
}