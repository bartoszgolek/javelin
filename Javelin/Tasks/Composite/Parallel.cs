using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javelin.Base.Tasks;

namespace Javelin.Tasks.Composite
{
	internal class Parallel : Base.Tasks.Task<ParallelConfig>
	{
		public Parallel(string id, ParallelConfig config, ITaskFactory taskFactory)
			: base(id, config)
		{
			this.taskFactory = taskFactory;
		}

		public override TaskResult Run()
		{
			var results = new List<TaskResult>();

			var taskConfigs = config.TaskConfigs.ToList();

			var tasks = taskConfigs.Select(tc => new Task(() => results.Add(taskFactory.CreateTask(tc).Run()))).ToList();

			tasks.ForEach(t => t.Start());
			Task.WaitAll(tasks.ToArray());

			var description = string.Join(Environment.NewLine, results.Select(r => r.Description));
			if (results.All(r => r.Status == TaskResultStatus.Success))
				return TaskResult.Success(description);

			if (results.Any(r => r.Status == TaskResultStatus.Failed))
				return TaskResult.Failed(description);

			return TaskResult.Warning(description);
		}

		private readonly ITaskFactory taskFactory;
	}
}