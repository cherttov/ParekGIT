using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    processInfo.FileName = "cmd.exe";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    processInfo.FileName = "open";
                    processInfo.Arguments = $"-a Terminal \"{repoPath}\"";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    processInfo.FileName = "x-terminal-emulator";
                }

                Process.Start(processInfo);
            }

            return Task.CompletedTask;
        }
    }
}