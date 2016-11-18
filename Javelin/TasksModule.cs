using Autofac;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;
using Javelin.Tasks;
using Javelin.Tasks.Backup;
using Javelin.Tasks.Composite;
using Javelin.Tasks.Misc;
using Javelin.Tasks.WindowsService;

namespace Javelin
{
	public class TasksModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<TasksJobFactory>();
			builder.RegisterType<TaskFactory>().As<ITaskFactory>();
			builder.RegisterType<TaskConfigReader>().As<ITaskConfigReader>();

			builder.RegisterType<List>();
			builder.RegisterType<Parallel>();
			builder.RegisterType<Tasks.MasterSlave.Delegate>();
			builder.RegisterType<ZipFiles>();
			builder.RegisterType<DeleteOldFiles>();
			builder.RegisterType<StartWindowsService>();
			builder.RegisterType<StopWindowsService>();
			builder.RegisterType<PredefinedTask>();
			builder.RegisterType<EmptyTask>();
			builder.RegisterType<SleepTask>();
		}
	}
}