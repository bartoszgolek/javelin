using Javelin.Api.CommonContract;
using Javelin.Api.Rest;

namespace Javelin.Api.Slave
{
	public class SlaveServiceClient : RestClient, ISlaveServiceClient
	{
		public SlaveServiceClient(IRestReader reader, string host)
			: base(reader, host, ContentTypes.Json)
		{
		}

		public void DelegateTask(string delegationId, string taskDefinition)
		{
			var taskDelegate = new TaskDelegate
			{
				DelegationId = delegationId,
				TaskDefinition = taskDefinition
			};
			PostRequest<string, TaskDelegate>(string.Format("task/delegate/{0}", delegationId), taskDelegate);
		}
	}
}