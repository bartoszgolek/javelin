using System.IO;
using Newtonsoft.Json.Linq;

namespace Javelin.Base
{
	public class JsonConfig
	{
		public JObject Config { get; private set; }

		public JsonConfig(string filePath)
		{
			string configJson;
			using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (var sr = new StreamReader(fs))
			{
				configJson = sr.ReadToEnd();
			}

			Config = JObject.Parse(configJson);
		}
	}
}