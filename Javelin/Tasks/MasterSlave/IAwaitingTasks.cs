using System;
using Javelin.Base.Tasks;

namespace Javelin.Tasks.MasterSlave
{
	public interface IAwaitingTasks
	{
		TaskResult Wait(string delegationId, Action action, TimeSpan timeout);

		void SetResult(string delegationId, TaskResult result);
	}
}