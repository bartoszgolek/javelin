using Javelin.Base.Tasks;

namespace Javelin.Api.Master
{
	public interface IMasterServiceClient
	{
		void TaskFinished(string delegationId, TaskResult taskResult);
	}
}