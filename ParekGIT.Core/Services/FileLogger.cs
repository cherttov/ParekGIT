using ParekGIT.Core.Interfaces;
using System.Runtime.InteropServices;

namespace ParekGIT.Core.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _logDirectory;
        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);
        private const int MaxLogFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public FileLogger()
        {
            _logDirectory = AppDataPaths.GetLogDirectory();
            EnsureLogDirectoryExists();
        }

        public async Task LogErrorAsync(string message, Exception? exception = null)
        {
            string fullMessage = FormatLogMessage("ERROR", message, exception);
            await WriteLogAsync(fullMessage);

        }

        public async Task LogWarningAsync(string message)
        {
            string fullMessage = FormatLogMessage("WARNING", message, null);
            await WriteLogAsync(fullMessage);
        }

        public async Task LogInfoAsync(string message)
        {
            string fullMessage = FormatLogMessage("INFO", message, null);
            await WriteLogAsync(fullMessage);
        }

        // Private helpers
        private async Task WriteLogAsync(string message)
        {
            await _writeLock.WaitAsync();
            try
            {
                string logFile = Path.Combine(_logDirectory, $"parekgit-{DateTime.UtcNow:dd-MM-yyyy}.log");

                if (File.Exists(logFile) && new FileInfo(logFile).Length > MaxLogFileSizeBytes)
                {
                    string backupFile = Path.Combine(_logDirectory, $"parekgit-{DateTime.UtcNow:dd-MM-yyyy}_{DateTime.UtcNow:HHmmss}.bak");
                    File.Move(logFile, backupFile);
                }

                await File.AppendAllTextAsync(logFile, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGGER_ERROR] Failed to write to log file: {ex.Message}");
                Console.WriteLine($"[LOGGER] Original message: {message}");
            }
            finally
            {
                _writeLock.Release();
            }
        }

        private void EnsureLogDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(_logDirectory))
                {
                    Directory.CreateDirectory(_logDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create log directory at {_logDirectory}: {ex.Message}");
            }
        }

        private static string FormatLogMessage(string level, string message, Exception? exception)
        {
            string timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss");
            string formatted = $"[{timestamp}][{level}] {message}";

            if (exception != null)
            {
                formatted += Environment.NewLine + exception;
            }

            formatted += Environment.NewLine + new string('-', 80);

            return formatted;
        }
    }
}
