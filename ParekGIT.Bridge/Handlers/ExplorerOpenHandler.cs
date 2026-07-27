using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ExplorerOpenHandler : IMessageHandler
	{
		private readonly IFileSystemService _fileSystem;

		public string Action => "EXPLORER_OPEN";

		// Constructor
		public ExplorerOpenHandler(IFileSystemService fileSystem)
		{
			_fileSystem = fileSystem;
		}

		public Task ExecuteAsync(JsonElement payload)
		{
			string rawPath = payload.GetProperty("path").GetString()
				?? throw new IpcPayloadException("path");

			if (string.IsNullOrEmpty(rawPath)) { return Task.CompletedTask; }

			if (rawPath == "%APP_DATA%")
			{
				rawPath = AppDataPaths.GetLogDirectory();

				if (!_fileSystem.DirectoryExists(rawPath))
				{
					throw new Exception("Couldn't resolve the path of ParekGIT local app data.");
				}
			}

			string normalizedPath = Path.GetFullPath(rawPath);

			if (!_fileSystem.DirectoryExists(normalizedPath)) { return Task.CompletedTask; }

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				Process.Start("xdg-open", normalizedPath);
			}
			else
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = normalizedPath,
					UseShellExecute = true
				});
			}
			return Task.CompletedTask;
		}
	}
}
