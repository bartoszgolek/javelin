using System.Collections.Generic;
using Autofac;
using Javelin.Base;

namespace Javelin.Tasks.Perspectiv
{
	internal class TasksComponent : IComponent
	{
		public void Activate(ILifetimeScope scope)
		{
		}

		public void Deactivate(ILifetimeScope scope)
		{
		}

		public IEnumerable<Module> GetModules()
		{
			yield return new TasksModule();
		}

		public string Name
		{
			get { return "Javelin.Tasks.Perspectiv.TasksComponent"; }
		}
	}
}