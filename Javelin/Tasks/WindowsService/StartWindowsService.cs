using System;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.WindowsService
{
	public class StartWindowsService : Task<StartWindowsServiceConfig>
	{
		public StartWindowsService(string id, StartWindowsServiceConfig config)
			: base(id, config)
		{
		}

		public override TaskResult Run()
		{
			var logger = LogManager.GetLogger(GetType());

			try
			{
				logger.InfoFormat("Starting service '{0}'", config.ServiceName);
				ServiceManager.StartService(config.ServiceName, config.Timeout);
				logger.InfoFormat("Service '{0}' started.", config.ServiceName);
			}
			catch (Exception e)
			{
				logger.Error(string.Format("Error during starting service '{0}'.", config.ServiceName), e);
				return TaskResult.Failed(e.Message);
			}

			return TaskResult.Success();
		}
	}
}