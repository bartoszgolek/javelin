namespace Javelin.Api.Slave
{
	public interface ISlaveService
	{
		void RunDelegatedTask(string delegationId, string taskDefinition);
	}
}