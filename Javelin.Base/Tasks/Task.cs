using System;
using Javelin.Base.Config;
using log4net;
using Quartz;

namespace Javelin.Base.Tasks
{
	public interface ITask
	{
		TaskResult Run();
	}

	public abstract class Task<TConfiguration> : IJob, ITask where TConfiguration : BaseConfig
	{
		private readonly string taskId;
		protected TConfiguration config;

		protected Task(string id, TConfiguration config)
		{
			this.config = config;
			taskId = id;
		}

		public string TaskId
		{
			get { return taskId; }
		}

		public abstract TaskResult Run();

		public void Execute(JobExecutionContext context)
		{
			var logger = LogManager.GetLogger(GetType());
			try
			{
				var taskResult = Run();
				if (taskResult.Status == TaskResultStatus.Warning)
					logger.Warn(taskResult.Description);
				else if (taskResult.Status == TaskResultStatus.Failed)
					logger.Error(taskResult.Description);
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("Error during running task '{0}.", TaskId), ex);
			}
		}
	}
}