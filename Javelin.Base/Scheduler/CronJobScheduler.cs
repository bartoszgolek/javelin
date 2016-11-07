using System;
using log4net;
using Quartz;

namespace Javelin.Base.Scheduler
{
	public class CronJobScheduler<TJob, TJobConfig>
		where TJob : IJob
		where TJobConfig : IJob
	{
		public CronJobScheduler(IScheduler scheduler, ICronJobSchedulerConfig<TJobConfig> config = null)
		{
			this.scheduler = scheduler;
			this.config = config;

			log = LogManager.GetLogger(GetType());
		}

		public bool IsActive
		{
			get { return config != null && config.IsActive; }
		}

		public void Start()
		{
			ScheduleCronJob();

			if (config != null && config.StartImmediately)
				ScheduleImmediateJob();
		}

		public void Stop()
		{ }

		private void ScheduleCronJob()
		{
			string jobName = JobName;
			string jobGroup = jobName + "Group";
			string cronExpression = config != null ? config.Configuration : "";

			var trigger = new CronTrigger(jobName, jobGroup, cronExpression)
				{
					StartTimeUtc = DateTime.UtcNow,
					MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing
				};

			var job = new JobDetail(jobName, typeof(TJob));
			scheduler.ScheduleJob(job, trigger);

			log.InfoFormat("{0} scheduled with cron expression {1}", jobName, cronExpression);
		}

		private void ScheduleImmediateJob()
		{
			string jobName = JobName + "Immediate";
			var updateJob = new JobDetail(jobName, typeof(TJob));

			var trigger = TriggerUtils.MakeImmediateTrigger(jobName, 0, TimeSpan.MaxValue);
			trigger.StartTimeUtc = DateTime.UtcNow.AddSeconds(3);

			scheduler.ScheduleJob(updateJob, trigger);
		}

		private string JobName
		{
			get { return typeof(TJobConfig).Name; }
		}

		private readonly ICronJobSchedulerConfig<TJobConfig> config;
		private readonly IScheduler scheduler;
		private readonly ILog log;
	}

	public class CronJobScheduler<T> : CronJobScheduler<T, T>
		where T : IJob
	{
		public CronJobScheduler(IScheduler scheduler, ICronJobSchedulerConfig<T> config = null)
			: base(scheduler, config)
		{
		}
	}
}