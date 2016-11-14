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
			return new SchedulerConfig(configReader.GetSubconfigs("scheduler").Single(s => s["taskId"] == name));
		}

		public ITaskConfig GetTaskConfig(string taskId)
		{
			return new TaskConfig(configReader.GetSubconfigs("tasks").Single(t => t["id"] == taskId));
		}

		public ITaskConfig[] GetTaskConfigs()
		{
			return configReader.GetSubconfigs("tasks").Select(t => new TaskConfig(t)).Cast<ITaskConfig>().ToArray();
		}
	}
}