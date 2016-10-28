using Autofac;
using Quartz;
using Quartz.Spi;

namespace Javelin.Base.Scheduler
{
	/// <summary>
	/// Custom job factory used for creating jobs with autofac.
	/// Acctually it creates AutofacSelfOwnedJob which is used for using
	/// OwnedInstances ( autofac feature )
	/// to handle job scope.
	/// </summary>
	/// <example>
	/// Example registration of a job in autofac container.
	/// <code>
	/// builder.RegisterGeneric(typeof(AutofacSelfOwnedJob&lt;&gt;));
	/// builder.RegisterType&lt;SomeJob&gt;();
	/// </code>
	/// </example>
	public class AutofacJobFactory : IJobFactory
	{
		/// <summary>
		/// </summary>
		/// <param name="componentContext"></param>
		public AutofacJobFactory(IComponentContext componentContext)
		{
			this.componentContext = componentContext;
		}

		/// <summary>
		/// Called by the scheduler at the time of the trigger firing, in order to
		/// produce a <see cref="T:Quartz.IJob"/> instance on which to call Execute.
		/// </summary>
		/// <remarks>
		/// <p>It should be extremely rare for this method to throw an exception -
		/// basically only the the case where there is no way at all to instantiate
		/// and prepare the Job for execution.  When the exception is thrown, the
		/// Scheduler will move all triggers associated with the Job into the
		/// <see cref="F:Quartz.TriggerState.Error"/> state, which will require human
		/// intervention (e.g. an application restart after fixing whatever
		/// configuration problem led to the issue wih instantiating the Job.
		/// </p>
		/// </remarks>
		/// <param name="bundle">The TriggerFiredBundle from which the <see cref="T:Quartz.JobDetail"/>
		/// and other info relating to the trigger firing can be obtained.
		/// </param><throws>SchedulerException if there is a problem instantiating the Job. </throws>
		/// <returns>
		/// the newly instantiated Job
		/// </returns>
		public IJob NewJob(TriggerFiredBundle bundle)
		{
			var type = typeof(AutofacSelfOwnedJob<>).MakeGenericType(bundle.JobDetail.JobType);
			return (IJob)componentContext.Resolve(type);
		}

		private readonly IComponentContext componentContext;
	}
}