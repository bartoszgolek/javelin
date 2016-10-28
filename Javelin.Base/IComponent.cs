using System.Collections.Generic;
using Autofac;

namespace Javelin.Base
{
	public interface IComponent
	{
		/// <summary>
		/// Method is called after Perspectiv Server start
		/// </summary>
		/// <param name="scope"></param>
		void Activate(ILifetimeScope scope);

		/// <summary>
		/// Method is run before Perspectiv Server shutdown
		/// </summary>
		/// <param name="scope"></param>
		void Deactivate(ILifetimeScope scope);

		/// <summary>
		/// Returns modules which will configure Autofac container
		/// </summary>
		/// <returns></returns>
		IEnumerable<Module> GetModules();

		/// <summary>
		/// Returns component name
		/// </summary>
		string Name { get; }
	}
}