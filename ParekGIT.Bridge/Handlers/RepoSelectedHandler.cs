using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Core.Services;
using ParekGIT.Data.Data;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoSelectedHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IGitRunner _gitRunner;
        private readonly RepoWatcher _repoWatcher;
        private readonly IFileSystemService _fileSystem;

        public string Action => "REPO_SELECTED";

        // Constructor
        public RepoSelectedHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner, RepoWatcher repoWatcher, IFileSystemService fileSystem)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
            _repoWatcher = repoWatcher;
            _fileSystem = fileSystem;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("absolutePath").GetString()
                              ?? throw new IpcPayloadException("absolutePath");

            // Failed response
            if (!_fileSystem.DirectoryExists(repoPath))
            {
                await _dbStore.UpdateRepositoryValidStateAsync(repoPath, false);

                // Make an IPC message that path is missing to resolve
                var missingResponse = new IpcMessage
                {
                    Action = "REPO_PATH_MISSING",
                    Payload = JsonSerializer.SerializeToElement(new { absolutePath = repoPath })
                };
                _window.SendWebMessage(JsonSerializer.Serialize(missingResponse));
                return;
            }

            UserSettings currentSettings = await _dbStore.GetUserSettingsAsync();

            // Get branches
            IEnumerable<GitBranch> branches = await _gitRunner.GetBranchesAsync(repoPath);
            _repoWatcher.WatchRepository(repoPath);

            // Update last repo path
            if (currentSettings.LastRepoPath != repoPath)
            {
                currentSettings.LastRepoPath = repoPath;
                await _dbStore.SaveUserSettingsAsync(currentSettings);
            }

            // Response
            var response = new IpcMessage
            {
                Action = "BRANCHES_LOADED",
                Payload = JsonSerializer.SerializeToElement(branches)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
