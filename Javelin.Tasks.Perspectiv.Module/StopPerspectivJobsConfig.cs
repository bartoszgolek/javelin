using System;
using Javelin.Base.Config;

namespace Javelin.Tasks.Perspectiv
{
	internal class StopPerspectivJobsConfig : BaseConfig
	{
		public StopPerspectivJobsConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public string PerspectivUri
		{
			get { return configReader.GetValue("perspectivUri"); }
		}

		public TimeSpan Timeout
		{
			get { return configReader.GetTimeSpan("checkingInterval", TimeSpan.FromSeconds(5)); }
		}
	}
}