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
			get { return configReader["perspectivUri"]; }
		}
	}
}