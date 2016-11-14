using System;
using System.Threading.Tasks;
using Javelin.Api.WebService.Master;
using Javelin.Base.Config;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;
using log4net;
using Newtonsoft.Json.Linq;

namespace Javelin.Api.WebService.Slave
{
	internal class SlaveService : ISlaveService
	{
		private readonly ITaskFactory taskFactory;
		private readonly IMasterServiceClient masterClient;
		private readonly ILog logger;

		public SlaveService(ITaskFactory taskFactory, IMasterServiceClient masterClient)
		{
			this.taskFactory = taskFactory;
			this.masterClient = masterClient;
			logger = LogManager.GetLogger(GetType());
		}

		public void RunDelegatedTask(string delegationId, string taskDefinition)
		{
			logger.DebugFormat("Received delegated task: '{0}'", taskDefinition);
			new Task(() =>
				{
					try
					{
						var task = taskFactory.CreateTask(new TaskConfig(new ConfigReader(JToken.Parse(taskDefinition))));
						masterClient.TaskFinished(delegationId, task.Run());
					}
					catch (Exception ex)
					{
						logger.Error(string.Format("Exception during execution of delagated task: '{0}'", delegationId), ex);
						masterClient.TaskFinished(delegationId, TaskResult.Failed(ex.Message));
					}
				}).Start();
		}
	}
}