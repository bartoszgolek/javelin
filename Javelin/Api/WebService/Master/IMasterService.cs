using Javelin.Base.Tasks;

namespace Javelin.Api.WebService.Master
{
	public interface IMasterService
	{
		void TaskFinished(string delegationId, TaskResult result);
	}
}