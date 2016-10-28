namespace Javelin.ScheduledTasks
{
	public interface ISchedulerConfig
	{
		string TaskId { get; }
		string CronExpression { get; }
		bool Locked { get; }
	}
}