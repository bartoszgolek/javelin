using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	public interface ITaskCronJobSchedulerConfig
	{
		ISchedulerConfig[] Scheduler { get; }
		bool HasScheduler { get; }
		ITaskConfig[] Tasks { get; }
	}
}