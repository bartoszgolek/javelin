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
			get { return configReader.GetEnum("mode", BootstrapperMode.Master) == BootstrapperMode.Slave; }
		}

		public bool IsMaster
		{
			get { return configReader.GetEnum("mode", BootstrapperMode.Master) == BootstrapperMode.Master; }
		}

		public string RootUri
		{
			get { return configReader.GetValue("rootUri"); }
		}
	}
}