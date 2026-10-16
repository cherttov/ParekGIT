using CliWrap;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Services
{
	public class FileSystemService : IFileSystemService
	{
		private readonly ILogger _logger;

		// Constructor
		public FileSystemService(ILogger logger)
		{
			_logger = logger;
		}

		// Exist
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		// Create
		public Task CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);

			return Task.CompletedTask;
		}

		// Write/read
		public Task<string> ReadAllTextAsync(string path)
		{
			return File.ReadAllTextAsync(path);
		}

		public Task WriteAllTextAsync(string path, string content)
		{
			return File.WriteAllTextAsync(path, content);
		}

		// Getters
		public async Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption)
		{
			return await Task.Run(() =>
				Directory.GetFileSystemEntries(path, searchPattern, searchOption)
			);
		}

		// Delete/recycle-bin
		public async Task MoveDirectoryToRecycleBinAsync(string path)
		{
			var directory = new DirectoryInfo(path);

			// Strip Read-Only attributes
			directory.Attributes = FileAttributes.Normal;
			foreach (var info in directory.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
			{
				try
				{
					info.Attributes = FileAttributes.Normal;
				}
				catch (UnauthorizedAccessException) { }
			}

			// Match OS and delete accordingly
			if (OperatingSystem.IsWindows())
			{
				FileSystem.DeleteDirectory(
					path,
					UIOption.OnlyErrorDialogs,
					RecycleOption.SendToRecycleBin);
			}
			else if (OperatingSystem.IsMacOS())
			{
				string escapedPath = path.Replace("\"", "\\\"");
				await Cli.Wrap("osascript")
					.WithArguments(["-e", $"tell application \"Finder\" to delete POSIX file \"{escapedPath}\""])
					.ExecuteAsync();
			}
			else if (OperatingSystem.IsLinux())
			{
				try
				{
					await Cli.Wrap("gio")
						.WithArguments(["trash", path])
						.ExecuteAsync();
				}
				catch (Exception ex)
				{
					await _logger.LogWarningAsync(
						$"'gio trash' unavailable when deleting '{path}'. Falling back to permanent delete. ({ex.Message})"
					);
					throw new InvalidOperationException($"Failed to move directory to a recycle bin. 'gio trash' might be missing.", ex);
				}
			}
		}

		public Task DeleteFile(string path)
		{
			File.Delete(path);
			return Task.CompletedTask;
		}
	}
}
