using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	public interface ITaskConfigReader
	{
		object GetTaskConfig(ITaskConfig taskConfig);
	}
}