using System;
using System.Linq;
using log4net;
using Quartz;

namespace Javelin.ScheduledTasks
{
	public class TaskCronJobScheduler : ITaskCronJobScheduler
	{
		public TaskCronJobScheduler(IScheduler scheduler, ITaskCronJobSchedulerConfig config)
		{
			this.scheduler = scheduler;
			this.config = config;
		}

		public bool IsActive
		{
			get { return config != null && config.HasScheduler && config.ShouldStartScheduler; }
		}

		public void Start()
		{
			ScheduleCronJob();
		}

		public void Stop()
		{ }

		private void ScheduleCronJob()
		{
			foreach (ISchedulerConfig schedulerConfig in config.Scheduler)
			{
				string jobName = schedulerConfig.TaskId;
				if (string.IsNullOrEmpty(jobName))
					Log.Warn("No task to run. Skipping.");

				var task = config.Tasks.SingleOrDefault(t => t.TaskId == jobName);

				if (task == null)
				{
					Log.ErrorFormat("Task '{0}' does not exists. Skipping.", jobName);
					continue;
				}

				Type jobType = task.TaskType;
				string jobGroup = jobName + "Group";
				string cronExpression = schedulerConfig.CronExpression;

				var trigger = new CronTrigger(jobName, jobGroup, cronExpression)
					{
						StartTimeUtc = DateTime.UtcNow,
						MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing
					};

				var job = new JobDetail(jobName, jobType);
				scheduler.ScheduleJob(job, trigger);

				Log.InfoFormat("{0} scheduled with cron expression {1}", jobName, cronExpression);
			}
		}

		private readonly ITaskCronJobSchedulerConfig config;
		private readonly IScheduler scheduler;
		private static readonly ILog Log = LogManager.GetLogger(typeof(TaskCronJobScheduler));
	}
}