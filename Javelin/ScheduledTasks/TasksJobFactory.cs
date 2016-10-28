using System;
using Javelin.Base.Tasks;
using Quartz;
using Quartz.Spi;

namespace Javelin.ScheduledTasks
{
	public class TasksJobFactory : IJobFactory
	{
		public TasksJobFactory(ITasksJobFactoryConfig config, ITaskFactory taskFactory)
		{
			this.config = config;
			this.taskFactory = taskFactory;
		}

		public IJob NewJob(TriggerFiredBundle bundle)
		{
			var newJob = CreateTask(bundle.JobDetail.Name) as IJob;
			if (newJob == null)
				throw new Exception("Task have to implement IJob interface.");

			return newJob;
		}

		private ITask CreateTask(string taskId)
		{
			var task = taskFactory.CreateTask(config.GetTaskConfig(taskId));
			return config.GetSchedulerConfig(taskId).Locked
				? new LockedTask(taskId, task)
				: task;
		}

		private readonly ITasksJobFactoryConfig config;
		private readonly ITaskFactory taskFactory;
	}
}