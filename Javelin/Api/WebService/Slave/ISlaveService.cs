namespace Javelin.Api.WebService.Slave
{
	public interface ISlaveService
	{
		void RunDelegatedTask(string delegationId, string taskDefinition);
	}
}