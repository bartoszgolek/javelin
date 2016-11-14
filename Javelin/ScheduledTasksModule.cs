using System;
using Autofac;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;
using Javelin.Tasks;
using Javelin.Tasks.Backup;
using Javelin.Tasks.Composite;
using Javelin.Tasks.WindowsService;
using Quartz;

namespace Javelin
{
	public class ScheduledTasksModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			const string tasksScheduler = "TasksScheduler";

			builder.RegisterType<TasksJobFactory>();
			builder.RegisterType<TaskFactory>().As<ITaskFactory>();
			builder.RegisterType<TaskConfigReader>().As<ITaskConfigReader>();

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
			builder.RegisterType<TasksJobFactoryConfig>()
				.As<ITasksJobFactoryConfig>()
				.As<IPredefinedTasksConfig>()
				.As<ISchedulersConfig>();

			RegisterTaskTypes(builder);
		}

		private void RegisterTaskTypes(ContainerBuilder builder)
		{
			builder.RegisterType<List>();
			builder.RegisterType<Parallel>();
			builder.RegisterType<Tasks.MasterSlave.Delegate>();
			builder.RegisterType<ZipFiles>();
			builder.RegisterType<DeleteOldFiles>();
			builder.RegisterType<StartWindowsService>();
			builder.RegisterType<StopWindowsService>();
			builder.RegisterType<PredefinedTask>();
			builder.RegisterType<EmptyTask>();
		}
	}
}