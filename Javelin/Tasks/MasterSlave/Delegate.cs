using System;
using Javelin.Api.Slave;
using Javelin.Base.Tasks;
using log4net;
using Newtonsoft.Json.Linq;

namespace Javelin.Tasks.MasterSlave
{
	internal class Delegate : Task<DelegateConfig>
	{
		private readonly ISlaveServiceClient client;
		private readonly IAwaitingTasks awaitingTasks;
		private readonly ILog logger;

		public Delegate(string id, DelegateConfig config, Func<string, ISlaveServiceClient> clientFactory, IAwaitingTasks awaitingTasks)
			: base(id, config)
		{
			this.awaitingTasks = awaitingTasks;
			client = clientFactory(config.Uri);
			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
			logger.InfoFormat("Delegating task to '{0}'", config.Uri);

			var delegationId = Guid.NewGuid().ToString();
			var taskDefinition = JToken.Parse(config.TaskDefinition).ToString();
			TaskResult taskResult = awaitingTasks.Wait(delegationId, () => client.DelegateTask(delegationId, taskDefinition), config.Timeout);

			logger.Info("Finished");

			return taskResult;
		}
	}
}