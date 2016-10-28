using System;
using Javelin.Api.Slave;
using Javelin.Base.Tasks;
using Newtonsoft.Json.Linq;

namespace Javelin.Tasks.MasterSlave
{
	internal class Delegate : Task<DelegateConfig>
	{
		private readonly ISlaveServiceClient client;
		private readonly IAwaitingTasks awaitingTasks;

		public Delegate(string id, DelegateConfig config, Func<string, ISlaveServiceClient> clientFactory, IAwaitingTasks awaitingTasks)
			: base(id, config)
		{
			this.awaitingTasks = awaitingTasks;
			client = clientFactory(config.Uri);
		}

		public override TaskResult Run()
		{
			var delegationId = Guid.NewGuid().ToString();
			var taskDefinition = JToken.Parse(config.TaskDefinition).ToString();
			return awaitingTasks.Wait(
				delegationId,
				() => client.DelegateTask(delegationId, taskDefinition),
				config.Timeout);
		}
	}
}