namespace Javelin.Api.Slave
{
	public interface ISlaveServiceClient
	{
		void DelegateTask(string delegationId, string taskDefinition);
	}
}