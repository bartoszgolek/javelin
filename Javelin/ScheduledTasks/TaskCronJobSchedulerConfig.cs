using System;
using System.Linq;
using Javelin.Base.Config;
using Javelin.Base.Tasks;
using Javelin.Config;

namespace Javelin.ScheduledTasks
{
	internal class TaskCronJobSchedulerConfig : BaseConfig, ITaskCronJobSchedulerConfig
	{
		public TaskCronJobSchedulerConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public bool ShouldStartScheduler
		{
			get { return (BootstrapperMode)Enum.Parse(typeof(BootstrapperMode), configReader.GetValue("mode")) == BootstrapperMode.Master; }
		}

		public bool HasScheduler
		{
			get { return configReader.Children("scheduler").Any(); }
		}

		public ISchedulerConfig[] Scheduler
		{
			get
			{
				return configReader
					.Children("scheduler")
					.Select(cr => new SchedulerConfig(cr))
					.Cast<ISchedulerConfig>()
					.ToArray();
			}
		}

		public ITaskConfig[] Tasks
		{
			get
			{
				return configReader
					.Children("tasks")
					.Select(cr => new TaskConfig(cr))
					.Cast<ITaskConfig>()
					.ToArray();
			}
		}
	}
}