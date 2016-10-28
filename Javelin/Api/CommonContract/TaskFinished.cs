using Javelin.Base.Tasks;

namespace Javelin.Api.CommonContract
{
	public class TaskFinished
	{
		public string DelegationId { get; set; }
		public TaskResult TaskResult { get; set; }
	}
}