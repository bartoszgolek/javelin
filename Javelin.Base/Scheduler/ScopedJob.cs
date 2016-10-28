using Autofac;
using Quartz;

namespace Javelin.Base.Scheduler
{
	public class ScopedJob<T> : IJob where T : IJob
	{
		private readonly ILifetimeScope lifetimeScope;

		public ScopedJob(ILifetimeScope lifetimeScope)
		{
			this.lifetimeScope = lifetimeScope;
		}

		public void Execute(JobExecutionContext context)
		{
			using (var scope = lifetimeScope.BeginLifetimeScope())
			{
				var job = scope.Resolve<T>();
				job.Execute(context);
			}
		}
	}
}