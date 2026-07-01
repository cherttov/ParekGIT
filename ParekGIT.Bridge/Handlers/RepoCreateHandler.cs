using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
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
                              ?? throw new IpcPayloadException("repoName");

            string localPath = payload.GetProperty("localPath").GetString()
                               ?? throw new IpcPayloadException("localPath");

            string gitIgnore = payload.GetProperty("gitIgnore").GetString()
                               ?? "None";

            string gitLicense = payload.GetProperty("gitLicense").GetString()
                               ?? "None";

            string licenseYear = payload.GetProperty("licenseYear").GetString()
                               ?? "";

            string licenseOrganization = payload.GetProperty("licenseOrganization").GetString()
                               ?? "";

            string licenseProject = payload.GetProperty("licenseProject").GetString()
                               ?? "";

            // Create repo in git
            var repository = await _gitRunner.CreateRepositoryAsync(
                repoName, localPath, gitIgnore, gitLicense, licenseYear, licenseOrganization, licenseProject
            );

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
