namespace Javelin.Api.WebService.Slave
{
	public interface ISlaveServiceClient
	{
		void DelegateTask(string delegationId, string taskDefinition);
	}
}