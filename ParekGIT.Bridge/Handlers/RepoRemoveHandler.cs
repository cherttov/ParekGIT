using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoRemoveHandler : IMessageHandler
    {
        PhotinoWindow _window;
        LiteDbStore _dbStore;
        IGitRunner _gitRunner;

        public string Action => "REPO_REMOVE";

        // Constructor
        public RepoRemoveHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            var allRepos = await _dbStore.GetAllRepositoriesAsync();
            var repoToDelete = allRepos.FirstOrDefault(repo =>
                string.Equals(repo.AbsolutePath, repoPath, StringComparison.OrdinalIgnoreCase));

            if (repoToDelete != null)
            {
                await _dbStore.DeleteRepositoryAsync(repoToDelete.Id);
            }

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_REMOVED",
                Payload = JsonSerializer.SerializeToElement(new { absolutePath = repoPath })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
