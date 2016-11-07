using System;
using Javelin.Base.Tasks;
using ServiceStack.Logging;

namespace Javelin.Tasks.Perspectiv
{
	internal class StopPerspectivJobs : Task<StopPerspectivJobsConfig>
	{
		private readonly Func<string, PerspectivManagementClient> perspectivManagementClientFactory;
		private readonly ILog logger;

		public StopPerspectivJobs(
			string id,
			StopPerspectivJobsConfig config,
			Func<string, PerspectivManagementClient> perspectivManagementClientFactory)
			: base(id, config)
		{
			this.perspectivManagementClientFactory = perspectivManagementClientFactory;
			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
			logger.InfoFormat("Stopping Perspectiv jobs: '{0}'", config.PerspectivUri);
			var managementClient = perspectivManagementClientFactory(config.PerspectivUri);
			managementClient.StopJobs();
			string[] activeJobs;
			do
			{
				activeJobs = managementClient.ListActiveJobs();
			} while (activeJobs.Length > 0);

			logger.Debug("Finished.");
			return TaskResult.Success();
		}
	}
}