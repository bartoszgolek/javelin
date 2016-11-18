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
				return configReader.GetBool("deleteFiles");
			}
		}

		public string ArchiveTemplate
		{
			get
			{
				return configReader.GetValue("archiveTemplate");
			}
		}

		public IDictionary<string, string> FileNames
		{
			get
			{
				return configReader.Children("files")
					.ToDictionary(cr => cr.GetValue("path"), cr => cr.GetValue("archivePath"));
			}
		}
	}
}