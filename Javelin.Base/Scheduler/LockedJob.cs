using System;
using System.Threading;
using Autofac;
using log4net;
using Quartz;

namespace Javelin.Base.Scheduler
{
	public class LockedJob<TJob> : IJob
		where TJob : IJob
	{
		public LockedJob(ILifetimeScope lifetimeScope)
		{
			this.lifetimeScope = lifetimeScope;
		}

		public void Execute(JobExecutionContext context)
		{
			if (!Monitor.TryEnter(ExecutionLock, 1))
			{
				Log.InfoFormat("{0} - skipped due to concurrent execution", JobName);
				return;
			}

			try
			{
				var innerJob = lifetimeScope.Resolve<TJob>();
				innerJob.Execute(context);
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("{0} - stopped with an exception: {1}", JobName, ex.Message), ex);
				throw;
			}
			finally
			{
				Monitor.Exit(ExecutionLock);
			}
		}

		private string JobName
		{
			get { return typeof(TJob).Name; }
		}

		private readonly ILifetimeScope lifetimeScope;

		// ReSharper disable StaticFieldInGenericType
		private static readonly object ExecutionLock = new object();

		private static readonly ILog Log = LogManager.GetLogger(typeof(LockedJob<TJob>));

		// ReSharper restore StaticFieldInGenericType
	}
}