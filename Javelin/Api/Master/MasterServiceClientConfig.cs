using Javelin.Base.Config;

namespace Javelin.Api.Master
{
	internal class MasterServiceClientConfig : BaseConfig, IMasterServiceClientConfig
	{
		public MasterServiceClientConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public string MasterUri
		{
			get { return configReader["masterUri"]; }
		}
	}
}