using System;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace Javelin.Api.Host
{
	public class ServiceStackThreadPoolManager : IDisposable
	{
		public ServiceStackThreadPoolManager(int workingThreadsMaxCount, int workingAndWaitingTasksMaxCount)
		{
			currentWorkingTasksCount = workingThreadsMaxCount;
			currentWorkingAndWaitingTasksCount = workingAndWaitingTasksMaxCount;
			initialWorkingAndWaitingTasksCount = workingAndWaitingTasksMaxCount;
			callbackQueue = new Queue<Tuple<WaitCallback, object>>();
		}

		public bool TryQueueWorkItem(WaitCallback callBack, object state)
		{
			if (isDisposing)
				return true;

			logger.InfoFormat("Queue length: {0}", initialWorkingAndWaitingTasksCount - currentWorkingAndWaitingTasksCount);
			logger.InfoFormat("Working threads: {0}", currentWorkingTasksCount);

			// Check if there is a place to process or queue the task
			if (Interlocked.Decrement(ref currentWorkingAndWaitingTasksCount) < 0)
			{
				// Restore previous value
				Interlocked.Increment(ref currentWorkingAndWaitingTasksCount);
				return false;
			}

			// Check if task can be processed immediately or must be queued
			if (Interlocked.Decrement(ref currentWorkingTasksCount) < 0)
			{
				// Restore previous value, no task is being executed at the moment.
				Interlocked.Increment(ref currentWorkingTasksCount);
				QueueWorkItem(callBack, state);
				return true;
			}

			// Queue task for execution
			ThreadPool.QueueUserWorkItem(delegate
			{
				callBack.Invoke(state);
				ProcessQueue();
				Interlocked.Increment(ref currentWorkingTasksCount);
				Interlocked.Increment(ref currentWorkingAndWaitingTasksCount);
			});
			return true;
		}

		private void QueueWorkItem(WaitCallback callback, object state)
		{
			lock (syncQueue)
			{
				callbackQueue.Enqueue(new Tuple<WaitCallback, object>(callback, state));
			}
		}

		private Tuple<WaitCallback, object> GetQueueWorkItem()
		{
			lock (syncQueue)
			{
				if (callbackQueue.Count > 0)
				{
					Interlocked.Increment(ref currentWorkingAndWaitingTasksCount);
					return callbackQueue.Dequeue();
				}
				return null;
			}
		}

		private void ProcessQueue()
		{
			var queuedTask = GetQueueWorkItem();
			while (queuedTask != null)
			{
				queuedTask.Item1.Invoke(queuedTask.Item2);
				queuedTask = GetQueueWorkItem();
			}
		}

		public void Dispose()
		{
			lock (this)
			{
				if (isDisposing)
					return;

				isDisposing = true;
			}
		}

		// For testing purpose only
		public bool ThreadsAreWorking
		{
			get
			{
				return currentWorkingAndWaitingTasksCount != initialWorkingAndWaitingTasksCount;
			}
		}

		private int currentWorkingTasksCount;
		private int currentWorkingAndWaitingTasksCount;

		private readonly object syncQueue = new object();
		private volatile bool isDisposing;

		//In original file there was AutoResetEven used, but it does not guarantee that two threads fred in a very
		// short time one after another will cause two threads to start working. There may be situation when one
		// thread will stay awaiting forever.
		private readonly int initialWorkingAndWaitingTasksCount;

		private readonly Queue<Tuple<WaitCallback, object>> callbackQueue;
		private readonly ILog logger = LogManager.GetLogger(typeof(ServiceStackThreadPoolManager));
	}
}