using System;
using Javelin.Base.Tasks;

namespace Javelin.Tasks.Perspectiv
{
	internal class StopPerspectivJobs : Task<StopPerspectivJobsConfig>
	{
		private readonly Func<string, PerspectivManagementClient> perspectivManagementClientFactory;

		public StopPerspectivJobs(
			string id,
			StopPerspectivJobsConfig config,
			Func<string, PerspectivManagementClient> perspectivManagementClientFactory)
			: base(id, config)
		{
			this.perspectivManagementClientFactory = perspectivManagementClientFactory;
		}

		public override TaskResult Run()
		{
			var managementClient = perspectivManagementClientFactory(config.PerspectivUri);
			managementClient.StopJobs();
			string[] activeJobs;
			do
			{
				activeJobs = managementClient.ListActiveJobs();
			} while (activeJobs.Length > 0);

			return TaskResult.Success();
		}
	}
}