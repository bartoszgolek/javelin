namespace Javelin.Base.Tasks
{
	public class TaskResult
	{
		public static TaskResult Success(string description = null)
		{
			return new TaskResult(TaskResultStatus.Success, description);
		}

		public static TaskResult Failed(string description = null)
		{
			return new TaskResult(TaskResultStatus.Failed, description);
		}

		public static TaskResult Warning(string description = null)
		{
			return new TaskResult(TaskResultStatus.Warning, description);
		}

		public TaskResult(TaskResultStatus status, string description)
		{
			Description = description;
			Status = status;
		}

		public TaskResultStatus Status { get; private set; }
		public string Description { get; private set; }
	}
}