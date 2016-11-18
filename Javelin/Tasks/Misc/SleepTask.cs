using System.Threading;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.Misc
{
	public class SleepTask : Task<SleepTaskConfig>
	{
		private ILog logger;

		public SleepTask(string id, SleepTaskConfig config)
			: base(id, config)
		{
			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
			var timeout = config.Timeout;
			logger.InfoFormat("Sleeping for '{0}'", timeout);
			Thread.Sleep(timeout);
			return TaskResult.Success();
		}
	}
}