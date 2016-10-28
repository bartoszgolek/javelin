using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Javelin.Api.Rest.Serializers
{
	public class JsonWebClientSerializer : IWebClientSerializer
	{
		private const int Utf8 = 65001;

		public void ToStream<T>(Stream stream, T entity)
		{
			var serializer = new JsonSerializer();
			serializer.Converters.Add(new JavaScriptDateTimeConverter());
			serializer.NullValueHandling = NullValueHandling.Ignore;

			using (var writer = new StreamWriter(stream))
				serializer.Serialize(writer, entity);
		}

		public string Serialize<T>(T entity)
		{
			var serializer = new JsonSerializer();
			serializer.Converters.Add(new JavaScriptDateTimeConverter());
			serializer.NullValueHandling = NullValueHandling.Ignore;

			using (var ms = new MemoryStream())
			{
				ToStream(ms, entity);
				ms.Position = 0;
				return new StreamReader(ms, Encoding.GetEncoding(Utf8)).ReadToEnd();
			}
		}

		public T Deserialize<T>(string entity)
		{
			var dcs = new JsonSerializer();
			using (var stringReader = new StringReader(entity))
			using (var jsonTextReader = new JsonTextReader(stringReader))
				return dcs.Deserialize<T>(jsonTextReader);
		}

		public T Deserialize<T>(Stream entityStream)
		{
			var dcs = new JsonSerializer();
			using (var stringReader = new StreamReader(entityStream))
			using (var jsonTextReader = new JsonTextReader(stringReader))
				return dcs.Deserialize<T>(jsonTextReader);
		}
	}
}