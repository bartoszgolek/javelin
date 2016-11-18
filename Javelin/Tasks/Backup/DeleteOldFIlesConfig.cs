using System.Collections.Generic;
using System.Linq;
using Javelin.Base.Config;

namespace Javelin.Tasks.Backup
{
	public class DeleteOldFilesConfig : BaseConfig
	{
		public DeleteOldFilesConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public int NumberOfDays
		{
			get
			{
				return configReader.GetInt32("numberOfDays");
			}
		}

		public string RegexFilter
		{
			get
			{
				return configReader.GetValue("regexFilter");
			}
		}

		public IList<string> Directories
		{
			get
			{
				return configReader
					.Children("directories")
					.Select(cr => cr.ToString())
					.ToList();
			}
		}
	}
}