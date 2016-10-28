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
				return int.Parse(configReader["numberOfDays"]);
			}
		}

		public string RegexFilter
		{
			get
			{
				return configReader["regexFilter"];
			}
		}

		public IList<string> Directories
		{
			get
			{
				return configReader
					.GetSubconfigs("directories")
					.Select(cr => cr.ToString())
					.ToList();
			}
		}
	}
}