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
					string? customTerminal = Environment.GetEnvironmentVariable("TERMINAL");

					if (!string.IsNullOrEmpty(customTerminal))
					{
						processInfo.FileName = customTerminal;
					} 
					else if (_fileSystem.FileExists("usr/bin/konsole"))
					{
						processInfo.FileName = "konsole";
					}
					else if (_fileSystem.FileExists("/usr/bin/gnome-terminal"))
					{
						processInfo.FileName = "gnome-terminal";
					}
					else
					{
						processInfo.FileName = "xterm";
					}

				}

				Process.Start(processInfo);
			}
			return Task.CompletedTask;
		}
	}
}