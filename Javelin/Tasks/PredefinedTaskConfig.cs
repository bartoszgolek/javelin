using Javelin.Base.Config;

namespace Javelin.Tasks
{
	internal class PredefinedTaskConfig : BaseConfig
	{
		public PredefinedTaskConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public string TaskId
		{
			get { return configReader.GetValue("taskId"); }
		}
	}
}