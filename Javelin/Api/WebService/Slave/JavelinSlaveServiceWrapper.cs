using Javelin.Api.CommonContract;
using Javelin.Api.Helpers;
using ServiceStack.ServiceHost;

namespace Javelin.Api.WebService.Slave
{
	public class JavelinSlaveServiceWrapper : IService
	{
		private readonly ISlaveService slaveService;
		private readonly IServiceStackTemplate<FaultDetails> serviceStackTemplate;

		public JavelinSlaveServiceWrapper(ISlaveService slaveService, IServiceStackTemplate<FaultDetails> serviceStackTemplate)
		{
			this.slaveService = slaveService;
			this.serviceStackTemplate = serviceStackTemplate;
		}

		public object Post(TaskDelegate taskDelegate)
		{
			return serviceStackTemplate.DoAction(() => slaveService.RunDelegatedTask(taskDelegate.DelegationId, taskDelegate.TaskDefinition));
		}
	}
}