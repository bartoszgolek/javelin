using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.Backup
{
	public class DeleteOldFiles : Task<DeleteOldFilesConfig>
	{
		private readonly IList<string> directories;
		private readonly int numberOfDays;
		private readonly string regexFilter;

		private readonly ILog logger;
		private readonly Regex regex;

		public DeleteOldFiles(string id, DeleteOldFilesConfig config)
			: base(id, config)
		{
			directories = config.Directories;
			numberOfDays = config.NumberOfDays;
			regexFilter = config.RegexFilter;

			logger = LogManager.GetLogger(GetType());
			regex = new Regex(regexFilter);
		}

		protected override TaskResult DoTask()
		{
			DeleteFiles();

			return TaskResult.Success();
		}

		private void DeleteFiles()
		{
			logger.InfoFormat("Deleteing files matching pattern '{0}' older than: '{1}' days in:{2}",
				regexFilter,
				numberOfDays,
				directories.Select(d => Environment.NewLine + " - " + d + ","));

			foreach (var directory in directories)
			{
				logger.DebugFormat("Deleteing from: {0}", directory);

				var files = Directory.GetFiles(directory)
					.Where(f =>
						{
							var fileName = Path.GetFileName(f);
							return fileName != null && regex.IsMatch(fileName);
						})
					.ToList();

				foreach (var file in files.Where(f => File.GetCreationTime(f).Date.AddDays(numberOfDays) <= DateTime.Now.Date))
				{
					logger.DebugFormat("{0}", file);
					File.Delete(file);
				}
			}

			logger.Info("Deleteing files finished.");
		}
	}
}