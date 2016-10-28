using Javelin.Base.Config;

namespace Javelin.Tasks.WindowsService
{
	public class StopWindowsServiceConfig : WindowsServiceConfigBase
	{
		public StopWindowsServiceConfig(IConfigReader configReader)
			: base(configReader)
		{
		}
	}
}