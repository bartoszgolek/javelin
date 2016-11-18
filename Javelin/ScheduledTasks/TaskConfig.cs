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
			get { return configReader.GetValue("id"); }
		}

		public string Description
		{
			get { return configReader.GetValue("description") ?? string.Empty; }
		}

		public Type TaskType
		{
			get
			{
				var type = Type.GetType(configReader.GetValue("type"));

				if (type == null)
					throw new Exception(string.Format("Unknown type '{0}'.", configReader.GetValue("type")));

				return type;
			}
		}

		public bool IsHidden
		{
			get { return configReader.GetBool("hidden"); }
		}

		public TTaskConfig GetTaskConfig<TTaskConfig>()
		{
			return (TTaskConfig)Activator.CreateInstance(typeof(TTaskConfig), configReader["configuration"]);
		}
	}
}