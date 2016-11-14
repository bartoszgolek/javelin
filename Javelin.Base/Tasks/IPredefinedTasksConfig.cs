namespace Javelin.Base.Tasks
{
	public interface IPredefinedTasksConfig
	{
		ITaskConfig GetTaskConfig(string taskId);

		ITaskConfig[] GetTaskConfigs();
	}
}