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
			taskId = id ?? Guid.NewGuid().ToString("B");
		}

		public string TaskId
		{
			get { return taskId; }
		}

		protected abstract TaskResult DoTask();

		public TaskResult Run()
		{
			var logger = LogManager.GetLogger(GetType());
			try
			{
				var taskResult = DoTask();
				if (taskResult.Status == TaskResultStatus.Warning)
					logger.Warn(taskResult.Description);
				else if (taskResult.Status == TaskResultStatus.Failed)
					logger.Error(taskResult.Description);

				return taskResult;
			}
			catch (Exception ex)
			{
				var message = string.Format("Error during running task '{0}.", TaskId);
				logger.Error(message, ex);
				return TaskResult.Failed(message);
			}
		}

		public void Execute(JobExecutionContext context)
		{
			Run();
		}
	}
}