using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Javelin.Base.Config
{
	public class ConfigReader : IConfigReader
	{
		public ConfigReader()
			: this("config.json")
		{
		}

		public ConfigReader(string path)
		{
			if (string.IsNullOrEmpty(path))
				path = "config.json";

			using (var configFile = new FileStream(path, FileMode.Open, FileAccess.Read))
			using (var configFileReader = new StreamReader(configFile))
			{
				plainConfig = configFileReader.ReadToEnd();
				configJson = JToken.Parse(plainConfig);
			}
		}

		public ConfigReader(JToken token)
		{
			configJson = token;
			plainConfig = token.ToString();
		}

		public string GetValue(string path)
		{
			var jToken = GetToken(path);
			if (jToken == null)
				return null;

			return jToken.Value<string>();
		}

		public IConfigReader this[string path]
		{
			get { return new ConfigReader(GetToken(path)); }
		}

		public IConfigReader[] Children(string path)
		{
			var jToken = GetToken(path);

			if (jToken == null)
				return new IConfigReader[0];

			if (!(jToken is JArray))
				throw new ArgumentException(string.Format("Cannot get configs for path '{0}'. Target element is not an array.", path), "path");

			return jToken.Select(t => new ConfigReader(t)).Cast<IConfigReader>().ToArray();
		}

		private JToken GetToken(string path)
		{
			JToken currentElement = configJson;
			foreach (var item in path.Split('.'))
			{
				if (currentElement is JArray)
				{
					int arrayIndex;
					if (!int.TryParse(item, out arrayIndex))
						throw new ArgumentException(string.Format("Path element '{0}' is invalid. Array position index expected.", item),
													"path");

					currentElement = currentElement[arrayIndex];
				}
				else if (currentElement is JObject)
				{
					currentElement = currentElement[item];
				}
				else
				{
					throw new ArgumentException("Path", "path");
				}
			}
			return currentElement;
		}

		public override string ToString()
		{
			return plainConfig;
		}

		private readonly JToken configJson;
		private readonly string plainConfig;
	}
}