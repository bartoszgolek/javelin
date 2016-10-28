using System;
using Autofac.Features.OwnedInstances;
using log4net;
using Quartz;

namespace Javelin.Base.Scheduler
{
	/// <summary>
	/// Quartz.Net IJob wrapped created for using
	/// <a href="http://code.google.com/p/autofac/wiki/OwnedInstances">Owned Instances</a>
	/// for job creation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AutofacSelfOwnedJob<T> : IJob where T : IJob
	{
		/// <summary>
		/// </summary>
		/// <param name="jobFactory">Functor which creates jobs.</param>
		public AutofacSelfOwnedJob(Func<Owned<T>> jobFactory)
		{
			this.jobFactory = jobFactory;
		}

		/// <summary>
		/// Uses injected JobFactory functor to create job and executes it in autofac scope.
		/// </summary>
		/// <param name="context"></param>
		public void Execute(JobExecutionContext context)
		{
			try
			{
				using (var ownedJob = jobFactory())
				{
					var theJob = ownedJob.Value;
					theJob.Execute(context);
				}
			}
			catch (Exception ex)
			{
				if (ex is JobExecutionException)
					throw;
				Logger.Error("Error while executing Cron IJob.", ex);
				throw new JobExecutionException(ex);
			}
		}

		private readonly Func<Owned<T>> jobFactory;
		private static readonly ILog Logger = LogManager.GetLogger(typeof(AutofacSelfOwnedJob<T>));
	}
}