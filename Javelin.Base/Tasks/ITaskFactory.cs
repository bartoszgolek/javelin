namespace Javelin.Base.Tasks
{
	public interface ITaskFactory
	{
		ITask CreateTask(ITaskConfig taskConfig);
	}
}