using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	public interface ITasksJobFactoryConfig : IPredefinedTasksConfig, ISchedulersConfig
	{
	}

	public interface ISchedulersConfig
	{
		ISchedulerConfig GetSchedulerConfig(string name);
	}
}