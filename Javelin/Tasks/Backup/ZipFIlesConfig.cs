using System.Collections.Generic;
using System.Linq;
using Javelin.Base.Config;

namespace Javelin.Tasks.Backup
{
	public class ZipFIlesConfig : BaseConfig
	{
		public ZipFIlesConfig(IConfigReader configReader)
			: base(configReader)
		{
		}

		public bool DeleteFiles
		{
			get
			{
				return bool.Parse(configReader["deleteFiles"]);
			}
		}

		public string ArchiveTemplate
		{
			get
			{
				return configReader["archiveTemplate"];
			}
		}

		public IDictionary<string, string> FileNames
		{
			get
			{
				return configReader.GetSubconfigs("files")
					.ToDictionary(cr => cr["path"], cr => cr["archivePath"]);
			}
		}
	}
}