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
    public class RepoCloneHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IGitRunner _gitRunner;

        public string Action => "REPO_CLONE";

        // Constructor
        public RepoCloneHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoUrl = payload.GetProperty("repoUrl").GetString()
                ?? throw new IpcPayloadException("repoUrl");

            bool asLocal = payload.GetProperty("asLocal").GetBoolean();

            string localPath = payload.GetProperty("localPath").GetString()
                ?? throw new IpcPayloadException("localPath");

            await _gitRunner.CloneRepositoryAsync(repoUrl, asLocal, localPath);

            string repoName = Path.GetFileName(localPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            var clonedRepo = new GitRepository
            {
                Id = Guid.NewGuid(),
                Name = repoName,
                AbsolutePath = localPath,
                LastAccessed = DateTime.Now,
                IsRemote = !asLocal
            };

            await _dbStore.UpsertRepositoryAsync(clonedRepo);

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_CLONED",
                Payload = JsonSerializer.SerializeToElement(clonedRepo)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
