using Autofac;
using Javelin.Api;
using Javelin.Api.CommonContract;
using Javelin.Api.Master;
using Javelin.Api.Rest;
using Javelin.Api.Slave;
using Javelin.Tasks.MasterSlave;

namespace Javelin
{
	public class MasterModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MasterService>().As<IMasterService>();
			builder.RegisterType<JavelinMasterServiceWrapper>();
			builder.RegisterType<SlaveServiceClient>().As<ISlaveServiceClient>().SingleInstance();
			builder.RegisterType<AwaitingTasks>().As<IAwaitingTasks>().SingleInstance();

			builder.RegisterRestOperation<TaskFinished>(MethodVerbs.Post, "/task/finished/{DelegationId}");
		}
	}
}