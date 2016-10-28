using System.Collections.Generic;
using System.Linq;
using ServiceStack.Configuration;

namespace Javelin.Api
{
	public class ServiceStackAppHost : ServiceStackAppHostBase
	{
		private readonly IEnumerable<ServiceStackMethodRegistrationInfo> registrations;

		public ServiceStackAppHost(IContainerAdapter containerAdapter, IEnumerable<ServiceStackMethodRegistrationInfo> registrations)
			: base(containerAdapter, "JavelinService", typeof(ServiceStackAppHost).Assembly)
		{
			this.registrations = registrations;
		}

		protected override ServiceStackMethodRegistrationInfo[] GetRegisters()
		{
			return registrations.ToArray();
		}
	}
}