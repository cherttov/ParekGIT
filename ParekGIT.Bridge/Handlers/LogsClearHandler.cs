using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class LogsClearHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IFileSystemService _fileSystem;
        private readonly ILogger _logger;

        public string Action => "LOGS_CLEAR";

        public LogsClearHandler(PhotinoWindow window, IFileSystemService fileSystem, ILogger logger)
        {
            _window = window;
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string logsDirectory = AppDataPaths.GetLogDirectory();

            int deletedCount = 0;
            int failedCount = 0;

            if (_fileSystem.DirectoryExists(logsDirectory))
            {
                IEnumerable<string> logFiles = await _fileSystem.GetFileSystemEntriesAsync(logsDirectory, "*.log", SearchOption.TopDirectoryOnly);
                IEnumerable<string> bakFiles = await _fileSystem.GetFileSystemEntriesAsync(logsDirectory, "*.bak", SearchOption.TopDirectoryOnly);
                IEnumerable<string> allFiles = logFiles.Concat(bakFiles);

                string todayLogFile = Path.Combine(logsDirectory, $"parekgit-{DateTime.UtcNow:dd-MM-yyyy}.log");

                foreach (string file in allFiles)
                {
                    if (string.Equals(file, todayLogFile, StringComparison.OrdinalIgnoreCase)) { continue; }

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
            var response = new IpcMessage
            {
                Action = "LOGS_CLEARED",
                Payload = JsonSerializer.SerializeToElement(new { deletedCount, failedCount })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
