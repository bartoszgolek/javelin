using Autofac;
using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	public class TaskFactory : ITaskFactory
	{
		public TaskFactory(IComponentContext container, ITaskConfigReader taskConfigReader)
		{
			this.container = container;
			this.taskConfigReader = taskConfigReader;
		}

		public ITask CreateTask(ITaskConfig taskConfig)
		{
			var config = taskConfigReader.GetTaskConfig(taskConfig);
			return container.Resolve(
				taskConfig.TaskType,
				new TypedParameter(typeof(string), taskConfig.TaskId),
				new TypedParameter(config.GetType(), config)) as ITask;
		}

		private readonly IComponentContext container;
		private readonly ITaskConfigReader taskConfigReader;
	}
}