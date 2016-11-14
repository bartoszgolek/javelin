using Javelin.Api.CommonContract;
using Javelin.Api.Helpers;
using ServiceStack.ServiceHost;

namespace Javelin.Api.WebService.Master
{
	public class JavelinMasterServiceWrapper : IService
	{
		private readonly IMasterService masterService;
		private readonly IServiceStackTemplate<FaultDetails> serviceStackTemplate;

		public JavelinMasterServiceWrapper(IMasterService masterService, IServiceStackTemplate<FaultDetails> serviceStackTemplate)
		{
			this.masterService = masterService;
			this.serviceStackTemplate = serviceStackTemplate;
		}

		public object Post(TaskFinished taskFinished)
		{
			return serviceStackTemplate.DoAction(() => masterService.TaskFinished(taskFinished.DelegationId, taskFinished.TaskResult));
		}
	}
}