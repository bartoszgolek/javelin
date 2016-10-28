using System;
using System.Collections.Generic;
using System.Threading;
using Javelin.Base.Tasks;

namespace Javelin.Tasks.MasterSlave
{
	public class AwaitingTasks : IAwaitingTasks
	{
		private static readonly IDictionary<string, Waiting> Awaitings = new Dictionary<string, Waiting>();
		private static readonly object AwaitingsMutex = new object();

		public TaskResult Wait(string delegationId, Action action, TimeSpan timeout)
		{
			var waiting = new Waiting();
			lock (AwaitingsMutex)
				Awaitings.Add(delegationId, waiting);

			action();

			try
			{
				return waiting.Wait(timeout);
			}
			catch (Exception)
			{
				lock (AwaitingsMutex)
					Awaitings.Remove(delegationId);
				throw;
			}
		}

		public void SetResult(string delegationId, TaskResult result)
		{
			lock (AwaitingsMutex)
			{
				if (!Awaitings.ContainsKey(delegationId))
					return;

				var waiting = Awaitings[delegationId];
				Awaitings.Remove(delegationId);
				waiting.Release(result);
			}
		}

		private class Waiting
		{
			private readonly AutoResetEvent autoResetEvent;

			private TaskResult result;

			public Waiting()
			{
				autoResetEvent = new AutoResetEvent(false);
			}

			public TaskResult Wait(TimeSpan timeout)
			{
				autoResetEvent.WaitOne(timeout);

				if (result == null)
					throw new TimeoutException();

				return result;
			}

			public void Release(TaskResult taskResult)
			{
				result = taskResult;
				autoResetEvent.Set();
			}
		}
	}
}