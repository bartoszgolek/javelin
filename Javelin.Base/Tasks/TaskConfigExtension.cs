namespace Javelin.Base.Tasks
{
	public static class TaskConfigExtension
	{
		public static string GetTaskInfo(this ITaskConfig taskConfig)
		{
			var taskInfo = taskConfig.TaskId ?? string.Empty;

			if (!string.IsNullOrEmpty(taskConfig.Description))
				taskInfo += ": " + taskConfig.Description;

			taskInfo += " [" + taskConfig.TaskType + "]";
			return taskInfo.TrimStart();
		}
	}
}