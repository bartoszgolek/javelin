using Javelin.Base.Config;

namespace Javelin.ScheduledTasks
{
	internal class SchedulerConfig : ISchedulerConfig
	{
		private readonly IConfigReader configReader;

		public SchedulerConfig(IConfigReader configReader)
		{
			this.configReader = configReader;
		}

		public string TaskId
		{
			get { return configReader["taskId"]; }
		}

		public string CronExpression
		{
			get { return configReader["cronExpression"]; }
		}

		public bool Locked
		{
			get { return bool.Parse(configReader["lock"]); }
		}
	}
}