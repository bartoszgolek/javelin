using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Javelin.Api.Rest.Serializers
{
	public class XmlWebClientSerializer : IWebClientSerializer
	{
		private const int Utf8 = 65001;

		public void ToStream<T>(Stream stream, T entity)
		{
			var dcs = new DataContractSerializer(typeof(T));
			dcs.WriteObject(stream, entity);
		}

		public string Serialize<T>(T entity)
		{
			var ms = new MemoryStream();
			ToStream(ms, entity);
			ms.Position = 0;

			return new StreamReader(ms, Encoding.GetEncoding(Utf8)).ReadToEnd();
		}

		public T Deserialize<T>(string entity)
		{
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			{
				sw.Write(entity);
				sw.Flush();
				ms.Position = 0;

				return Deserialize<T>(ms);
			}
		}

		public T Deserialize<T>(Stream entityStream)
		{
			var dcs = new DataContractSerializer(typeof(T));
			return (T)dcs.ReadObject(entityStream);
		}
	}
}