using Javelin.Base.Tasks;

namespace Javelin.Api.Master
{
	public interface IMasterService
	{
		void TaskFinished(string delegationId, TaskResult result);
	}
}