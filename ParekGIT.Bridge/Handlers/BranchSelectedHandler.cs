using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class BranchSelectedHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IGitRunner _gitRunner;

        public string Action => "BRANCH_SELECTED";

        public BranchSelectedHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("absolutePath").GetString()
                              ?? throw new ArgumentNullException("absolutePath");

            var response = new IpcMessage
            {
                Action = "BRANCH_LOADED",
                Payload = JsonSerializer.SerializeToElement(null)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
