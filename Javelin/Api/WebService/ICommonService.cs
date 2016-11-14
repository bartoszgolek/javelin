using Javelin.Base.Tasks;

namespace Javelin.Api.WebService
{
	public interface ICommonService
	{
		TaskResult RunTask(string taskId);

		string[] GetDefinedTasks();
	}
}