using System;
using Javelin.Base.Config;

namespace Javelin.Tasks.Misc
{
	public class SleepTaskConfig : BaseConfig
	{
		public SleepTaskConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public TimeSpan Timeout
		{
			get { return configReader.GetTimeSpan("timeout"); }
		}
	}
}