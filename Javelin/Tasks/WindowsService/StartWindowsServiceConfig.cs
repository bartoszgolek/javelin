using Javelin.Base.Config;

namespace Javelin.Tasks.WindowsService
{
	public class StartWindowsServiceConfig : WindowsServiceConfigBase
	{
		public StartWindowsServiceConfig(IConfigReader configReader)
			: base(configReader)
		{
		}
	}
}