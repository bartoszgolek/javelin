using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	public interface ITasksJobFactoryConfig
	{
		ISchedulerConfig GetSchedulerConfig(string name);

		ITaskConfig GetTaskConfig(string name);
	}
}