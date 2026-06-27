using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class AppReadyHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IFileSystemService _fileSystem;

        public string Action => "APP_READY";

        // Constructor
        public AppReadyHandler(PhotinoWindow window, LiteDbStore dbStore, IFileSystemService fileSystem)
        {
            _window = window;
            _dbStore = dbStore;
            _fileSystem = fileSystem;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            IEnumerable<GitRepository> repos = await _dbStore.GetAllRepositoriesAsync();

            // Check if invalid repos are still invalid
            foreach (GitRepository repo in repos)
            {
                bool isValidNow = _fileSystem.DirectoryExists(repo.AbsolutePath);

                if (repo.IsValid != isValidNow)
                {
                    repo.IsValid = isValidNow;
                    await _dbStore.UpsertRepositoryAsync(repo);
                }
            }

            UserSettings settings = await _dbStore.GetUserSettingsAsync();

            // Data, frontend ready
            var bootData = new
            {
                Repositories = repos,
                Settings = settings
            };

            // Response
            var response = new IpcMessage
            {
                Action = "APP_INITIALIZED",
                Payload = JsonSerializer.SerializeToElement(bootData)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
