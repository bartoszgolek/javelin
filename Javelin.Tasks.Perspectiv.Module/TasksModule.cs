using Autofac;

namespace Javelin.Tasks.Perspectiv
{
	internal class TasksModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<StopPerspectivJobs>();
			builder.RegisterType<PerspectivManagementClient>();
		}
	}
}