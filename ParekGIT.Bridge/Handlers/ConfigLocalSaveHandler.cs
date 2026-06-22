using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Git;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParekGIT.Bridge.Handlers
{
    public class ConfigLocalSaveHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IGitRunner _gitRunner;

        public string Action => "CONFIG_LOCAL_SAVE";

        // Constructor
        public ConfigLocalSaveHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                              ?? throw new ArgumentNullException("repoPath");

            string name = payload.GetProperty("name").GetString()
                              ?? throw new ArgumentNullException("name");

            string email = payload.GetProperty("email").GetString()
                              ?? throw new ArgumentNullException("email");

            await _gitRunner.SaveLocalConfigAsync(repoPath, name, email);

            // Response
            var response = new IpcMessage
            {
                Action = "CONFIG_LOCAL_SAVED",
                Payload = JsonSerializer.SerializeToElement("")
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
