using Autofac;
using Autofac.Core;
using log4net;
using ServiceStack.Configuration;

namespace Javelin.Api.Host
{
	public class AutofacIocAdapter : IContainerAdapter
	{
		public AutofacIocAdapter(ILifetimeScope scope)
		{
			this.scope = scope;
		}

		public T Resolve<T>()
		{
			T result;
			if (scope.TryResolve(out result))
			{
				return result;
			}

			var message = string.Format("Failed to resolve ServiceStack REST service dependency: {0}", typeof(T).FullName);
			Logger.Error(message);
			throw new DependencyResolutionException(message);
		}

		public T TryResolve<T>()
		{
			T result;
			return scope.TryResolve(out result) ? result : default(T);
		}

		private readonly ILifetimeScope scope;
		private static readonly ILog Logger = LogManager.GetLogger(typeof(AutofacIocAdapter));
	}
}