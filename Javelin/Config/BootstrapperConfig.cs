using System;
using Javelin.Base.Config;

namespace Javelin.Config
{
	internal class BootstrapperConfig : BaseConfig, IBootstrapperConfig
	{
		public BootstrapperConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public bool IsSlave
		{
			get { return (BootstrapperMode)Enum.Parse(typeof(BootstrapperMode), configReader["mode"]) == BootstrapperMode.Slave; }
		}

		public bool IsMaster
		{
			get { return (BootstrapperMode)Enum.Parse(typeof(BootstrapperMode), configReader["mode"]) == BootstrapperMode.Master; }
		}

		public string RootUri
		{
			get { return configReader["rootUri"]; }
		}
	}
}