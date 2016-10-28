using System;

namespace Javelin.Base.Tasks
{
	public interface ITaskConfig
	{
		string TaskId { get; }
		Type TaskType { get; }

		TTaskConfig GetTaskConfig<TTaskConfig>();
	}
}