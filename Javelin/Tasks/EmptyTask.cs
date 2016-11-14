using Javelin.Base.Tasks;

namespace Javelin.Tasks
{
	internal class EmptyTask : Task<EmptyTaskConfig>
	{
		public EmptyTask(string id, EmptyTaskConfig config)
			: base(id, config)
		{
		}

		protected override TaskResult DoTask()
		{
			return TaskResult.Success();
		}
	}
}