namespace Javelin.Base.Config
{
	public abstract class BaseConfig
	{
		protected readonly IConfigReader configReader;

		protected BaseConfig(IConfigReader configReader)
		{
			this.configReader = configReader;
		}
	}
}