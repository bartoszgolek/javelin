using System;

namespace Javelin.Base.Config
{
	public static class ConfigReaderExtensions
	{
		public static TimeSpan GetTimeSpan(this IConfigReader configReader, string path, TimeSpan defaultValue = new TimeSpan())
		{
			var value = configReader.GetValue(path);
			return value != null
				? TimeSpan.Parse(value)
				: defaultValue;
		}

		public static int GetInt32(this IConfigReader configReader, string path, int defaultValue = 0)
		{
			var value = configReader.GetValue(path);
			return value != null
				? int.Parse(value)
				: defaultValue;
		}

		public static bool GetBool(this IConfigReader configReader, string path, bool defaultValue = false)
		{
			var value = configReader.GetValue(path);
			return value != null
				? bool.Parse(value)
				: defaultValue;
		}

		public static TEnum GetEnum<TEnum>(this IConfigReader configReader, string path, TEnum defaultValue = default(TEnum))
		{
			var value = configReader.GetValue(path);
			return value != null
				? (TEnum)Enum.Parse(typeof(TEnum), configReader.GetValue(path))
				: defaultValue;
		}
	}
}