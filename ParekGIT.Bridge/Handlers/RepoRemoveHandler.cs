using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoRemoveHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IFileSystemService _fileSystem;
        private readonly ILogger _logger;

        public string Action => "REPO_REMOVE";

        // Constructor
        public RepoRemoveHandler(PhotinoWindow window, LiteDbStore dbStore, IFileSystemService fileSystem, ILogger logger)
        {
            _window = window;
            _dbStore = dbStore;
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new IpcPayloadException("repoPath");

            bool deleteLocal = payload.GetProperty("deleteLocal").GetBoolean();

            IEnumerable<GitRepository> allRepos = await _dbStore.GetAllRepositoriesAsync();
            GitRepository? repoToDelete = allRepos.FirstOrDefault(repo =>
                string.Equals(repo.AbsolutePath, repoPath, StringComparison.OrdinalIgnoreCase));

            // Delete from database
            if (repoToDelete != null)
            {
                await _dbStore.DeleteRepositoryAsync(repoToDelete.Id);
            }

            // Delete from drive
            bool localDeleteFailed = false;
            string? localDeleteError = null;

            if (deleteLocal && _fileSystem.DirectoryExists(repoPath))
            {
                try
                {
                    await _fileSystem.MoveDirectoryToRecycleBinAsync(repoPath);
                }
                catch (IOException ioEx)
                {
                    localDeleteFailed = true;
                    localDeleteError = "Directory might be in use.";
                    await _logger.LogErrorAsync($"Could not delete repo at '{repoPath}'. Directory might be in use.", ioEx);
                }
                catch (UnauthorizedAccessException unAuthEx)
                {
                    localDeleteFailed = true;
                    localDeleteError = "Access denied.";
                    await _logger.LogErrorAsync($"Access denied while deleting repo at '{repoPath}'.", unAuthEx);
                }
                catch (Exception ex)
                {
                    localDeleteFailed = true;
                    localDeleteError = ex.Message;
                    await _logger.LogErrorAsync($"Error while deleting repo at '{repoPath}'.", ex);
                }
            }

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_REMOVED",
                Payload = JsonSerializer.SerializeToElement(new 
                { 
                    absolutePath = repoPath,
                    localDeleteFailed,
                    localDeleteError
                })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
