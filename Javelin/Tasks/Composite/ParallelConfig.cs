using System.Linq;
using Javelin.Base.Config;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;

namespace Javelin.Tasks.Composite
{
	public class ParallelConfig : BaseConfig
	{
		public ParallelConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public ITaskConfig[] TaskConfigs
		{
			get
			{
				return configReader
					.Children("tasks")
					.Select(cr => new TaskConfig(cr))
					.Cast<ITaskConfig>()
					.ToArray();
			}
		}
	}
}