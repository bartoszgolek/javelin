using System;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.WindowsService
{
	public class StopWindowsService : Task<StopWindowsServiceConfig>
	{
		public StopWindowsService(string id, StopWindowsServiceConfig config)
			: base(id, config)
		{
		}

		public override TaskResult Run()
		{
			var logger = LogManager.GetLogger(GetType());

			try
			{
				logger.InfoFormat("Stopping service '{0}'", config.ServiceName);
				ServiceManager.StopService(config.ServiceName, config.Timeout);
				logger.InfoFormat("Service '{0}' stopped.", config.ServiceName);
			}
			catch (Exception e)
			{
				logger.Error(string.Format("Error during stopping service '{0}'.", config.ServiceName), e);
				return TaskResult.Failed(e.Message);
			}

			return TaskResult.Success();
		}
	}
}