using System.Threading.Tasks;
using Javelin.Api.Master;
using Javelin.Base.Config;
using Javelin.Base.Tasks;
using Javelin.ScheduledTasks;
using Newtonsoft.Json.Linq;

namespace Javelin.Api.Slave
{
	internal class SlaveService : ISlaveService
	{
		private readonly ITaskFactory taskFactory;
		private readonly IMasterServiceClient masterClient;

		public SlaveService(ITaskFactory taskFactory, IMasterServiceClient masterClient)
		{
			this.taskFactory = taskFactory;
			this.masterClient = masterClient;
		}

		public void RunDelegatedTask(string delegationId, string taskDefinition)
		{
			new Task(() =>
				{
					var task = taskFactory.CreateTask(new TaskConfig(new ConfigReader(JToken.Parse(taskDefinition))));
					masterClient.TaskFinished(delegationId, task.Run());
				}).Start();
		}
	}
}