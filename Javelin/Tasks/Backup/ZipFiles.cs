using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Javelin.Base.Tasks;
using log4net;

namespace Javelin.Tasks.Backup
{
	public class ZipFiles : Task<ZipFIlesConfig>
	{
		private readonly IDictionary<string, string> fileNames;
		private readonly bool deleteFiles;
		private readonly string archiveTemplate;
		private readonly ILog logger;

		public ZipFiles(string id, ZipFIlesConfig config)
			: base(id, config)
		{
			fileNames = base.config.FileNames;
			deleteFiles = base.config.DeleteFiles;
			archiveTemplate = base.config.ArchiveTemplate;

			logger = LogManager.GetLogger(GetType());
		}

		protected override TaskResult DoTask()
		{
			CompressFiles();

			return TaskResult.Success();
		}

		private void CompressFiles()
		{
			var archive = archiveTemplate.Replace("{timestamp}", DateTime.Now.ToString("yyyyMMdd_HHmm"));

			logger.InfoFormat("Compressing into '{0}' files:{1}",
				archive,
				fileNames.Select(fn => Environment.NewLine + " - " + fn + ","));

			CreateArchive(archive);
			DeleteFiles();

			logger.Debug("Finished");
		}

		private void CreateArchive(string archive)
		{
			logger.DebugFormat("Creating archive '{0}'.", archive);

			FileStream fsOut = File.Create(archive);
			var zipStream = new ZipOutputStream(fsOut);

			zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

			logger.Debug("Adding files:");

			foreach (KeyValuePair<string, string> fileEntry in fileNames)
			{
				if (!File.Exists(fileEntry.Key))
				{
					LogManager.GetLogger(GetType()).WarnFormat("File '{0}' not exists. Skipping.", fileEntry.Key);
					continue;
				}

				logger.DebugFormat("Adding file '{0}", fileEntry.Key);

				var fi = new FileInfo(fileEntry.Key);

				string entryName = fileEntry.Value;
				entryName = ZipEntry.CleanName(entryName);
				var newEntry = new ZipEntry(entryName)
					{
						DateTime = fi.LastWriteTime,
						Size = fi.Length
					};

				zipStream.PutNextEntry(newEntry);

				var buffer = new byte[4096];
				using (FileStream streamReader = File.OpenRead(fileEntry.Key))
					StreamUtils.Copy(streamReader, zipStream, buffer);

				zipStream.CloseEntry();
			}

			zipStream.IsStreamOwner = true;
			zipStream.Close();

			logger.Debug("Compressing finished.");
		}

		private void DeleteFiles()
		{
			if (deleteFiles)
			{
				logger.Debug("Deleting compressed files:");
				foreach (string filePath in fileNames.Keys)
				{
					logger.Debug(filePath);
					File.Delete(filePath);
				}

				logger.Debug("Deleting Finished:");
			}
		}
	}
}