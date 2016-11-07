using System;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.WindowsService
{
	public class StopWindowsService : Task<StopWindowsServiceConfig>
	{
		private readonly ILog logger;

		public StopWindowsService(string id, StopWindowsServiceConfig config)
			: base(id, config)
		{
			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
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