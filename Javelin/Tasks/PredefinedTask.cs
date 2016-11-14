using Javelin.Base.Tasks;

namespace Javelin.Tasks
{
	internal class PredefinedTask : Task<PredefinedTaskConfig>
	{
		public PredefinedTask(string id, PredefinedTaskConfig config, ITaskFactory taskFactory, IPredefinedTasksConfig tasksConfig)
			: base(id, config)
		{
			this.taskFactory = taskFactory;
			this.tasksConfig = tasksConfig;
		}

		protected override TaskResult DoTask()
		{
			ITaskConfig taskConfig = tasksConfig.GetTaskConfig(config.TaskId);
			return taskFactory.CreateTask(taskConfig).Run();
		}

		private readonly ITaskFactory taskFactory;
		private readonly IPredefinedTasksConfig tasksConfig;
	}
}