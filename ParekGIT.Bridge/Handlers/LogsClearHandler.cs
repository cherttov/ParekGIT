using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Core.Helpers;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class LogsClearHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IFileSystemService _fileSystem;
		private readonly IAppDataPaths _appDataPaths;
		private readonly ILogger _logger;

		public string Action => "LOGS_CLEAR";

		// Constructor
		public LogsClearHandler(PhotinoWindow window, IFileSystemService fileSystem, IAppDataPaths appDataPaths, ILogger logger)
		{
			_window = window;
			_fileSystem = fileSystem;
			_appDataPaths = appDataPaths;
			_logger = logger;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string logsDirectory = _appDataPaths.GetLogDirectory();

			int deletedCount = 0;
			int failedCount = 0;

			if (_fileSystem.DirectoryExists(logsDirectory))
			{
				IEnumerable<string> logFiles = await _fileSystem.GetFileSystemEntriesAsync(logsDirectory, "*.log", SearchOption.TopDirectoryOnly);
				IEnumerable<string> bakFiles = await _fileSystem.GetFileSystemEntriesAsync(logsDirectory, "*.bak", SearchOption.TopDirectoryOnly);
				IEnumerable<string> allFiles = logFiles.Concat(bakFiles);

				string todayLogFile = Path.Combine(logsDirectory, $"parekgit-{DateTime.UtcNow:yyyy-MM-dd}.log");

				foreach (string file in allFiles)
				{
					if (PathHelper.AreEqual(file, todayLogFile)) continue;

					try
					{
						await _fileSystem.DeleteFile(file);
						deletedCount++;
					}
					catch (Exception ex)
					{
						failedCount++;
						await _logger.LogErrorAsync($"Failed to delete log file '{file}'.", ex);
					}
				}
			}

			// Response
			var response = IpcMessage.Create("LOGS_CLEARED", new { deletedCount, failedCount });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
