using Javelin.Base.Tasks;

namespace Javelin.Api.WebService.Master
{
	public interface IMasterServiceClient
	{
		void TaskFinished(string delegationId, TaskResult taskResult);
	}
}