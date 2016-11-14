using Javelin.Api.CommonContract;
using Javelin.Api.Helpers;
using ServiceStack.ServiceHost;

namespace Javelin.Api.WebService
{
	public class JavelinCommonServiceWrapper : IService
	{
		private readonly ICommonService commonService;
		private readonly IServiceStackTemplate<FaultDetails> serviceStackTemplate;

		public JavelinCommonServiceWrapper(ICommonService commonService, IServiceStackTemplate<FaultDetails> serviceStackTemplate)
		{
			this.commonService = commonService;
			this.serviceStackTemplate = serviceStackTemplate;
		}

		public object Post(TaskRun taskRun)
		{
			return serviceStackTemplate.DoAction(() => commonService.RunTask(taskRun.TaskId));
		}

		public object Get(TasksGet taskGet)
		{
			return serviceStackTemplate.DoAction(() => commonService.GetDefinedTasks());
		}
	}
}