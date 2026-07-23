using CliWrap;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;
using ParekGIT.Core.Interfaces;
using System.Runtime.InteropServices;

namespace ParekGIT.Core.Services
{
	public class FileSystemService : IFileSystemService
	{
		private readonly ILogger _logger;

		public FileSystemService(ILogger logger)
		{
			_logger = logger;
		}

		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public Task<string> ReadAllTextAsync(string path)
		{
			return File.ReadAllTextAsync(path);
		}

		public Task WriteAllTextAsync(string path, string content)
		{
			return File.WriteAllTextAsync(path, content);
		}

		public async Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption)
		{
			return await Task.Run(() =>
				Directory.GetFileSystemEntries(path, searchPattern, searchOption)
			);
		}

		public async Task MoveDirectoryToRecycleBinAsync(string path)
		{
			var directory = new DirectoryInfo(path);

			// Strip Read-Only attributes
			directory.Attributes = FileAttributes.Normal;
			foreach (var info in directory.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
			{
				info.Attributes = FileAttributes.Normal;
			}

			// Match OS and delete accordingly
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				FileSystem.DeleteDirectory(
					path,
					UIOption.OnlyErrorDialogs,
					RecycleOption.SendToRecycleBin);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				await Cli.Wrap("osascript")
					.WithArguments(["-e", $"tell application \"Finder\" to delete POSIX file \"{path}\""])
					.ExecuteAsync();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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
						$"'gio trash' unavailable when deleting '{path}. Falling back to permanent delete. ({ex.Message})"
					);
					directory.Delete(true);
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
