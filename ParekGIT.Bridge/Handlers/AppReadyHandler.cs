using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
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

        public string Action => "APP_READY";

        // Constructor
        public AppReadyHandler(PhotinoWindow window, LiteDbStore dbStore)
        {
            _window = window;
            _dbStore = dbStore;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            IEnumerable<GitRepository> repos = await _dbStore.GetAllRepositoriesAsync();
            UserSettings settings = await _dbStore.GetUserSettingsAsync();

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
