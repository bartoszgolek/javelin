using System.Collections.Generic;
using Autofac;
using Javelin.Base;

namespace Javelin.ScheduledTasks
{
	internal class TasksSchedulerComponent : IComponent
	{
		public void Activate(ILifetimeScope scope)
		{
			var taskCronJobScheduler = scope.Resolve<ITaskCronJobScheduler>();
			if (taskCronJobScheduler.IsActive)
				taskCronJobScheduler.Start();
		}

		public void Deactivate(ILifetimeScope scope)
		{
			var taskCronJobScheduler = scope.Resolve<ITaskCronJobScheduler>();
			if (taskCronJobScheduler.IsActive)
				taskCronJobScheduler.Stop();
		}

		public IEnumerable<Module> GetModules()
		{
			yield return new ScheduledTasksModule();
		}

		public string Name { get { return "ScheduledTasks"; } }
	}
}