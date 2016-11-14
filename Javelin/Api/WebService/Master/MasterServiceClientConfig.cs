using Javelin.Base.Config;

namespace Javelin.Api.WebService.Master
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