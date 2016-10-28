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
			get { return config != null && config.HasScheduler; }
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
				Type jobType = config.Tasks.Single(t => t.TaskId == jobName).TaskType;
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