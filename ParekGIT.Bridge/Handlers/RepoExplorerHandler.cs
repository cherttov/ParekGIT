using ParekGIT.Bridge.Interfaces;
using System.Diagnostics;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoExplorerHandler : IMessageHandler
    {
        public string Action => "REPO_EXPLORER";

        public Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            if (!string.IsNullOrEmpty(repoPath) && Directory.Exists(repoPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = repoPath,
                    UseShellExecute = true
                });
            }

            return Task.CompletedTask;
        }
    }
}
