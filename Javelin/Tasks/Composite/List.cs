using System;
using System.Collections.Generic;
using System.Linq;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.Composite
{
	internal class List : Task<ListConfig>
	{
		private readonly ITaskFactory taskFactory;
		private readonly ILog logger;

		public List(string id, ListConfig config, ITaskFactory taskFactory)
			: base(id, config)
		{
			this.taskFactory = taskFactory;
			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
			var taskConfigs = config.TaskConfigs.ToList();
			if (!taskConfigs.Any())
				logger.InfoFormat("No tasks to run.");

			logger.InfoFormat("Runnig task list: {0}",
				string.Join("", taskConfigs.Select(t => Environment.NewLine + " - " + t.GetTaskInfo())));

			var results = new List<TaskResult>();
			taskConfigs.ForEach(tc =>
				{
					var taskInfo = tc.GetTaskInfo();

					logger.DebugFormat("Creating task: {0}", taskInfo);
					var task = taskFactory.CreateTask(tc);

					logger.DebugFormat("Runnig: {0}", taskInfo);
					results.Add(task.Run());
				});

			logger.DebugFormat("Finished.");

			var description = string.Join(Environment.NewLine, results.Select(r => r.Description).Where(s => !string.IsNullOrWhiteSpace(s)));
			if (results.All(r => r.Status == TaskResultStatus.Success))
				return TaskResult.Success(description);

			if (results.Any(r => r.Status == TaskResultStatus.Failed))
				return TaskResult.Failed(description);

			return TaskResult.Warning(description);
		}
	}
}