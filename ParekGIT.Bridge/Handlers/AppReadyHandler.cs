using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class AppReadyHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;

        public string Action => "APP_READY";

        public AppReadyHandler(PhotinoWindow window, LiteDbStore dbStore)
        {
            _window = window;
            _dbStore = dbStore;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            var repos = await _dbStore.GetAllRepositoriesAsync();

            var response = new IpcMessage
            {
                Action = "LOAD_REPOSITORIES",
                Payload = JsonSerializer.SerializeToElement(repos)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
