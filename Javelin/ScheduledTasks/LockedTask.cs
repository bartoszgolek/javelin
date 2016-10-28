using System;
using System.Threading;
using Javelin.Base.Tasks;
using log4net;
using Quartz;

namespace Javelin.ScheduledTasks
{
	public class LockedTask : IJob, ITask
	{
		public LockedTask(string jobName, ITask task)
		{
			innerTask = task;
			this.jobName = jobName;
		}

		public void Execute(JobExecutionContext context)
		{
			Run();
		}

		public TaskResult Run()
		{
			if (!Monitor.TryEnter(ExecutionLock, 1))
			{
				var m = string.Format("{0} - skipped due to concurrent execution", jobName);
				Log.Info(m);
				return TaskResult.Warning(m);
			}

			try
			{
				return innerTask.Run();
			}
			catch (Exception ex)
			{
				var m = string.Format("{0} - stopped with an exception: {1}", jobName, ex.Message);
				Log.Error(m, ex);
				throw;
			}
			finally
			{
				Monitor.Exit(ExecutionLock);
			}
		}

		private readonly string jobName;
		private readonly ITask innerTask;

		private static readonly object ExecutionLock = new object();
		private static readonly ILog Log = LogManager.GetLogger(typeof(LockedTask));
	}
}