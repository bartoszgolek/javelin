using System;
using Javelin.Base.Config;
using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	internal class TaskConfig : ITaskConfig
	{
		private readonly IConfigReader configReader;

		public TaskConfig(IConfigReader configReader)
		{
			this.configReader = configReader;
		}

		public string TaskId
		{
			get { return configReader["id"]; }
		}

		public Type TaskType
		{
			get
			{
				var type = Type.GetType(configReader["type"]);

				if (type == null)
					throw new Exception(string.Format("Unknown type '{0}'.", configReader["type"]));

				return type;
			}
		}

		public TTaskConfig GetTaskConfig<TTaskConfig>()
		{
			return (TTaskConfig)Activator.CreateInstance(typeof(TTaskConfig), configReader.GetSubconfig("configuration"));
		}
	}
}