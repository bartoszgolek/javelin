using System.Linq;
using Javelin.Base.Config;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;

namespace Javelin.Tasks.Composite
{
	public class ListConfig : BaseConfig
	{
		public ListConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public bool BreakOnFail
		{
			get
			{
				var breakOnFail = configReader["breakOnFail"];
				return breakOnFail == null || bool.Parse(breakOnFail);
			}
		}

		public ITaskConfig[] TaskConfigs
		{
			get
			{
				return configReader
					.GetSubconfigs("tasks")
					.Select(cr => new TaskConfig(cr))
					.Cast<ITaskConfig>()
					.ToArray();
			}
		}
	}
}