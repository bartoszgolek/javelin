using System;
using Javelin.Base.Config;

namespace Javelin.Tasks.WindowsService
{
	public abstract class WindowsServiceConfigBase : BaseConfig
	{
		protected WindowsServiceConfigBase(IConfigReader configReader)
			: base(configReader)
		{
		}

		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.Parse(configReader["timeout"]);
			}
		}

		public string ServiceName
		{
			get
			{
				return configReader["serviceName"];
			}
		}
	}
}