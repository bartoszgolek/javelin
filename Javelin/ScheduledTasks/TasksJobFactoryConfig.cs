using System.Linq;
using Javelin.Base.Config;
using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	internal class TasksJobFactoryConfig : BaseConfig, ITasksJobFactoryConfig
	{
		public TasksJobFactoryConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public ISchedulerConfig GetSchedulerConfig(string name)
		{
			return new SchedulerConfig(configReader.Children("scheduler").Single(s => s.GetValue("taskId") == name));
		}

		public ITaskConfig GetTaskConfig(string taskId)
		{
			return new TaskConfig(configReader.Children("tasks").Single(t => t.GetValue("id") == taskId));
		}

		public ITaskConfig[] GetTaskConfigs()
		{
			return configReader.Children("tasks").Select(t => new TaskConfig(t)).Cast<ITaskConfig>().ToArray();
		}
	}
}