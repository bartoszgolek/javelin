namespace Javelin.ScheduledTasks
{
	public interface ITaskCronJobScheduler
	{
		bool IsActive { get; }

		void Start();

		void Stop();
	}
}