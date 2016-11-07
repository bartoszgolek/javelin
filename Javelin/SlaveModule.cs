using Autofac;
using Javelin.Api;
using Javelin.Api.CommonContract;
using Javelin.Api.Master;
using Javelin.Api.Rest;
using Javelin.Api.Slave;

namespace Javelin
{
	public class SlaveModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<JavelinSlaveServiceWrapper>();

			builder.RegisterType<MasterServiceClient>().As<IMasterServiceClient>();
			builder.RegisterType<MasterServiceClientConfig>().As<IMasterServiceClientConfig>();
			builder.RegisterType<SlaveService>().As<ISlaveService>();

			builder.RegisterRestOperation<TaskDelegate>(MethodVerbs.Post, "/task/delegate/{DelegationId}");
		}
	}
}