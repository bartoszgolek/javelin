using Autofac;
using Javelin.Api;
using Javelin.Api.CommonContract;
using Javelin.Api.Rest;
using Javelin.Api.WebService;
using Javelin.Api.WebService.Master;
using Javelin.Api.WebService.Slave;
using Javelin.Tasks.MasterSlave;

namespace Javelin
{
	public class WebServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<JavelinMasterServiceWrapper>();
			builder.RegisterType<JavelinSlaveServiceWrapper>();
			builder.RegisterType<JavelinCommonServiceWrapper>();

			builder.RegisterType<MasterService>().As<IMasterService>();
			builder.RegisterType<SlaveService>().As<ISlaveService>();
			builder.RegisterType<CommonService>().As<ICommonService>();

			builder.RegisterType<MasterServiceClient>().As<IMasterServiceClient>();
			builder.RegisterType<MasterServiceClientConfig>().As<IMasterServiceClientConfig>();
			builder.RegisterType<SlaveServiceClient>().As<ISlaveServiceClient>();

			builder.RegisterRestOperation<TaskDelegate>(MethodVerbs.Post, "/task/delegate/{DelegationId}");
			builder.RegisterRestOperation<TaskFinished>(MethodVerbs.Post, "/task/finished/{DelegationId}");
			builder.RegisterRestOperation<TasksGet>(MethodVerbs.Get, "/tasks/");
			builder.RegisterRestOperation<TaskRun>(MethodVerbs.Post, "/task/run/{TaskId}");

			builder.RegisterType<AwaitingTasks>().As<IAwaitingTasks>().SingleInstance();
		}
	}
}