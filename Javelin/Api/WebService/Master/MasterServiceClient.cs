using Javelin.Api.CommonContract;
using Javelin.Api.Rest;
using Javelin.Base.Tasks;

namespace Javelin.Api.WebService.Master
{
	public class MasterServiceClient : RestClient, IMasterServiceClient
	{
		public MasterServiceClient(IRestReader reader, IMasterServiceClientConfig config)
			: base(reader, config.MasterUri, ContentTypes.Json)
		{
		}

		public void TaskFinished(string delegationId, TaskResult taskResult)
		{
			var taskFinished = new TaskFinished
				{
					DelegationId = delegationId,
					TaskResult = taskResult
				};
			PostRequest<string, TaskFinished>(string.Format("task/finished/{0}", delegationId), taskFinished);
		}
	}
}