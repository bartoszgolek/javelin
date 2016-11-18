using System;
using Autofac;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;
using Quartz;

namespace Javelin
{
	public class ScheduledTasksModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			const string tasksScheduler = "TasksScheduler";

			builder.RegisterType<TasksJobFactoryConfig>()
				.As<ITasksJobFactoryConfig>()
				.As<IPredefinedTasksConfig>()
				.As<ISchedulersConfig>();

			builder.Register(x =>
			{
				var scheduler = x.Resolve<ISchedulerFactory>().GetScheduler();
				scheduler.JobFactory = x.Resolve<TasksJobFactory>();
				return scheduler;
			}).SingleInstance().Named<IScheduler>(tasksScheduler).As<IScheduler>();

			builder.RegisterType<TaskCronJobScheduler>();
			builder
				.Register(c => c.Resolve<Func<IScheduler, TaskCronJobScheduler>>()(c.ResolveNamed<IScheduler>(tasksScheduler)))
				.As<ITaskCronJobScheduler>();

			builder.RegisterType<TaskCronJobSchedulerConfig>().As<ITaskCronJobSchedulerConfig>();
		}
	}
}