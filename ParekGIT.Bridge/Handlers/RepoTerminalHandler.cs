using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using System.Diagnostics;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoTerminalHandler : IMessageHandler
	{
		private readonly IFileSystemService _fileSystem;

		public string Action => "REPO_TERMINAL";

		// Constructor
		public RepoTerminalHandler(IFileSystemService fileSystem)
		{
			_fileSystem = fileSystem;
		}

		public Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			if (!string.IsNullOrEmpty(repoPath) && _fileSystem.DirectoryExists(repoPath))
			{
				var processInfo = new ProcessStartInfo
				{
					WorkingDirectory = repoPath,
					UseShellExecute = true
				};

				if (OperatingSystem.IsWindows())
				{
					processInfo.FileName = "cmd.exe";
				}
				else if (OperatingSystem.IsMacOS())
				{
					processInfo.FileName = "open";
					processInfo.Arguments = $"-a Terminal \"{repoPath}\"";
				}
				else if (OperatingSystem.IsLinux())
				{
					processInfo.FileName = "x-terminal-emulator";
				}

				Process.Start(processInfo);
			}
			return Task.CompletedTask;
		}
	}
}