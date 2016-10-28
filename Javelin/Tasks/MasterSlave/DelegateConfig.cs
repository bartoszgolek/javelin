using System;
using Javelin.Base.Config;

namespace Javelin.Tasks.MasterSlave
{
	internal class DelegateConfig : BaseConfig
	{
		public DelegateConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public string Uri
		{
			get { return configReader["uri"]; }
		}

		public string TaskDefinition
		{
			get { return configReader.GetSubconfig("task").ToString(); }
		}

		public TimeSpan Timeout
		{
			get { return TimeSpan.Parse(configReader["timeout"]); }
		}
	}
}