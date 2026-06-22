using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoCreateHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IGitRunner _gitRunner;

        public string Action => "REPO_CREATE";

        // Constructor
        public RepoCreateHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoName = payload.GetProperty("repoName").GetString()
                              ?? throw new ArgumentNullException("repoName");

            string localPath = payload.GetProperty("localPath").GetString()
                               ?? throw new ArgumentNullException("localPath");

            string gitIgnore = payload.GetProperty("gitIgnore").GetString()
                               ?? "None";

            string gitLicense = payload.GetProperty("gitLicense").GetString()
                               ?? "None";

            // Create repo in git
            var repository = await _gitRunner.CreateRepositoryAsync(repoName, localPath, gitIgnore, gitLicense);

            await _dbStore.UpsertRepositoryAsync(repository);

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_CREATED",
                Payload = JsonSerializer.SerializeToElement(repository)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
