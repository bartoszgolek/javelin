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
			get { return configReader.GetValue("taskId"); }
		}

		public string CronExpression
		{
			get { return configReader.GetValue("cronExpression"); }
		}

		public bool Locked
		{
			get { return configReader.GetBool("lock"); }
		}
	}
}