namespace Javelin.Base.Scheduler
{
	public interface ICronJobSchedulerConfig<T>
	{
		bool IsActive { get; }
		string Configuration { get; }
		bool StartImmediately { get; }
	}
}