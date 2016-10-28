using System;
using System.Collections.Generic;
using System.Linq;
using Javelin.Base.Tasks;

namespace Javelin.Tasks.Composite
{
	internal class List : Task<ListConfig>
	{
		private readonly ITaskFactory taskFactory;

		public List(string id, ListConfig config, ITaskFactory taskFactory)
			: base(id, config)
		{
			this.taskFactory = taskFactory;
		}

		public override TaskResult Run()
		{
			var results = new List<TaskResult>();

			var taskConfigs = config.TaskConfigs.ToList();

			taskConfigs.ForEach(tc => results.Add(taskFactory.CreateTask(tc).Run()));

			var description = string.Join(Environment.NewLine, results.Select(r => r.Description).Where(s => !string.IsNullOrWhiteSpace(s)));
			if (results.All(r => r.Status == TaskResultStatus.Success))
				return TaskResult.Success(description);

			if (results.Any(r => r.Status == TaskResultStatus.Failed))
				return TaskResult.Failed(description);

			return TaskResult.Warning(description);
		}
	}
}