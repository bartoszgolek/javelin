using System;
using System.Linq;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Api.WebService
{
	internal class CommonService : ICommonService
	{
		private readonly IPredefinedTasksConfig predefinedTasksConfig;
		private readonly ILog logger;
		private readonly ITaskFactory taskFactory;

		public CommonService(IPredefinedTasksConfig predefinedTasksConfig, ITaskFactory taskFactory)
		{
			this.predefinedTasksConfig = predefinedTasksConfig;
			this.taskFactory = taskFactory;
			logger = LogManager.GetLogger(GetType());
		}

		public string[] GetDefinedTasks()
		{
			return predefinedTasksConfig.GetTaskConfigs().Select(tc => tc.GetTaskInfo()).ToArray();
		}

		public TaskResult RunTask(string taskId)
		{
			try
			{
				logger.InfoFormat("Running task: '{0}'", taskId);

				ITaskConfig taskConfig = predefinedTasksConfig.GetTaskConfig(taskId);
				return taskFactory.CreateTask(taskConfig).Run();
			}
			catch (Exception ex)
			{
				var message = string.Format("Running task '{0}' failed.", taskId);
				logger.Info(message, ex);
				return TaskResult.Failed(message);
			}
		}
	}
}