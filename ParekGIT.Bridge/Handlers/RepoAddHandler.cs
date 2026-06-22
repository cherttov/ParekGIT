using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoAddHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;

        public string Action => "REPO_ADD";

        // Constructor
        public RepoAddHandler(PhotinoWindow window, LiteDbStore dbStore)
        {
            _window = window;
            _dbStore = dbStore;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new IpcPayloadException("repoPath");

            // Normalize path if ends with .git
            if (repoPath.EndsWith(".git", StringComparison.OrdinalIgnoreCase) ||
                repoPath.EndsWith(".git/", StringComparison.OrdinalIgnoreCase) ||
                repoPath.EndsWith(".git\\", StringComparison.OrdinalIgnoreCase))
            {
                repoPath = Directory.GetParent(repoPath.TrimEnd('/', '\\'))?.FullName ?? repoPath;
            }

            // Verify if valid .git repo
            string gitFolderPath = Path.Combine(repoPath, ".git");
            if (!Directory.Exists(gitFolderPath)) 
            { 
                throw new DirectoryNotFoundException("Selected folder is not a valid Git repository."); 
            }

            // Create repo object
            string repoName = new DirectoryInfo(repoPath).Name;
            DateTime lastAccessed = new DirectoryInfo(repoPath).LastAccessTime;

            var addedRepo = new GitRepository
            {
                Id = Guid.NewGuid(),
                Name = repoName,
                AbsolutePath = repoPath,
                LastAccessed = lastAccessed
            };

            IEnumerable<GitRepository> allRepos = await _dbStore.GetAllRepositoriesAsync();
            if (allRepos.Any(repo => string.Equals(repo.AbsolutePath, addedRepo.AbsolutePath, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Repository with this path already exists in the database.");
            }

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
    }
}
