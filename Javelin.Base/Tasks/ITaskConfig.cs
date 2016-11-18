using System;

namespace Javelin.Base.Tasks
{
	public interface ITaskConfig
	{
		string TaskId { get; }
		string Description { get; }
		Type TaskType { get; }
		bool IsHidden { get; }

		TTaskConfig GetTaskConfig<TTaskConfig>();
	}
}