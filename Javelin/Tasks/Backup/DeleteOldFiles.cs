using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Javelin.Base.Tasks;

namespace Javelin.Tasks.Backup
{
	public class DeleteOldFiles : Task<DeleteOldFilesConfig>
	{
		private readonly IList<string> directories;
		private readonly int numberOfDays;
		private readonly string regexFilter;

		public DeleteOldFiles(string id, DeleteOldFilesConfig config)
			: base(id, config)
		{
			directories = config.Directories;
			numberOfDays = config.NumberOfDays;
			regexFilter = config.RegexFilter;
		}

		public override TaskResult Run()
		{
			DeleteFiles();

			return TaskResult.Success();
		}

		private void DeleteFiles()
		{
			foreach (var directory in directories)
			{
				var regex = new Regex(regexFilter);
				var files = Directory.GetFiles(directory).ToList();
				files = files.Where(f => regex.IsMatch(Path.GetFileName(f))).ToList();

				foreach (var file in files)
				{
					var creationTime = File.GetCreationTime(file);
					if (creationTime.Date.AddDays(numberOfDays) <= DateTime.Now.Date)
						File.Delete(file);
				}
			}
		}
	}
}