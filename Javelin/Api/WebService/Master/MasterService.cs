using Javelin.Base.Tasks;
using Javelin.Tasks.MasterSlave;

namespace Javelin.Api.WebService.Master
{
	internal class MasterService : IMasterService
	{
		public MasterService(IAwaitingTasks awaitingTasks)
		{
			this.awaitingTasks = awaitingTasks;
		}

		public void TaskFinished(string delegationId, TaskResult result)
		{
			awaitingTasks.SetResult(delegationId, result);
		}

		private readonly IAwaitingTasks awaitingTasks;
	}
}