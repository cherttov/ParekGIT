using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoAddHandler : IMessageHandler
    {
        PhotinoWindow _window;
        LiteDbStore _dbStore;
        IGitRunner _gitRunner;

        public string Action => "REPO_ADD";

        public RepoAddHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string fullPath = payload.GetProperty("fullPath").GetString()
                ?? throw new ArgumentNullException("fullPath");

            // Normalize path if ends with .git
            if (fullPath.EndsWith(".git", StringComparison.OrdinalIgnoreCase) ||
                fullPath.EndsWith(".git/", StringComparison.OrdinalIgnoreCase) ||
                fullPath.EndsWith(".git\\", StringComparison.OrdinalIgnoreCase))
            {
                fullPath = Directory.GetParent(fullPath.TrimEnd('/', '\\'))?.FullName ?? fullPath;
            }

            // Verify if valid .git repo
            string gitFolderPath = Path.Combine(fullPath, ".git");
            if (!Directory.Exists(gitFolderPath))
            {
                SendError("The selected folder is not a valid Git repository.");
                return;
            }

            // Create repo object
            string repoName = new DirectoryInfo(fullPath).Name;
            DateTime lastAccessed = new DirectoryInfo(fullPath).LastAccessTime;

            var addedRepo = new GitRepository
            {
                Id = Guid.NewGuid(),
                Name = repoName,
                AbsolutePath = fullPath,
                LastAccessed = lastAccessed
            };

            // Save to db
            await _dbStore.UpsertRepositoryAsync(addedRepo);

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_ADDED",
                Payload = JsonSerializer.SerializeToElement(addedRepo)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }

        // Helpers
        private void SendError(string message)
        {
            // Response
            var errorResponse = new IpcMessage
            {
                Action = "REPO_ADD_ERROR",
                Payload = JsonSerializer.SerializeToElement(new { error = message })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(errorResponse));
        }
    }
}
