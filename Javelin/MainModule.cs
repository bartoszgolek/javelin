using Autofac;
using Javelin.Base.Config;
using Javelin.Base.Scheduler;
using Javelin.Config;
using Quartz;
using Quartz.Impl;

namespace Javelin
{
	internal class MainModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// Scheduler factory
			builder.RegisterInstance(new StdSchedulerFactory())
				.As<ISchedulerFactory>().SingleInstance();

			// job factory
			builder.RegisterType<AutofacJobFactory>();
			builder.RegisterGeneric(typeof(AutofacSelfOwnedJob<>));

			// ISchedule registration, more preferable then using ISchedulerFactory
			builder.Register(x =>
			{
				var scheduler = x.Resolve<ISchedulerFactory>().GetScheduler();
				scheduler.JobFactory = x.Resolve<AutofacJobFactory>();
				return scheduler;
			}).SingleInstance().As<IScheduler>();

			builder.RegisterGeneric(typeof(LockedJob<>));
			builder.RegisterGeneric(typeof(ScopedJob<>));

			builder.RegisterType<BootstrapperConfig>().As<IBootstrapperConfig>();
			builder.RegisterType<ConfigReader>().As<IConfigReader>();
		}
	}
}