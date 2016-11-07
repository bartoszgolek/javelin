using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.Composite
{
	internal class Parallel : Base.Tasks.Task<ParallelConfig>
	{
		public Parallel(string id, ParallelConfig config, ITaskFactory taskFactory)
			: base(id, config)
		{
			this.taskFactory = taskFactory;
			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
			const string noTasksMessage = "No tasks to run.";

			var results = new List<TaskResult>();

			var taskConfigs = config.TaskConfigs.ToList();
			if (!taskConfigs.Any())
			{
				logger.InfoFormat(noTasksMessage);
				return TaskResult.Warning(noTasksMessage);
			}

			logger.InfoFormat("Runnig parallel tasks: {0}",
				string.Join("", taskConfigs.Select(t => Environment.NewLine + " - " + t.GetTaskInfo())));

			var tasks = taskConfigs.Select(tc => new Task(() =>
				{
					var taskInfo = tc.GetTaskInfo();

					logger.DebugFormat("Creating task: {0}", taskInfo);
					var task = taskFactory.CreateTask(tc);

					logger.DebugFormat("Runnig: {0}", taskInfo);
					results.Add(task.Run());
				})).ToList();

			tasks.ForEach(t => t.Start());
			Task.WaitAll(tasks.ToArray());

			logger.DebugFormat("Finished.");

			var description = string.Join(Environment.NewLine, results.Select(r => r.Description));
			if (results.All(r => r.Status == TaskResultStatus.Success))
				return TaskResult.Success(description);

			if (results.Any(r => r.Status == TaskResultStatus.Failed))
				return TaskResult.Failed(description);

			return TaskResult.Warning(description);
		}

		private readonly ITaskFactory taskFactory;
		private readonly ILog logger;
	}
}