using Javelin.Base.Tasks;

namespace Javelin.Tasks.Composite
{
	public static class TaskConfigExtension
	{
		public static string GetTaskInfo(this ITaskConfig taskConfig)
		{
			return taskConfig.TaskId + " [" + taskConfig.TaskType + "]";
		}
	}
}