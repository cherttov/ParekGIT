using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
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
    public class ChangeIgnoreHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IGitRunner _gitRunner;

        public string Action => "CHANGE_IGNORE";

        // Constructor
        public ChangeIgnoreHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            string filePath = payload.GetProperty("filePath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            string ignoreType = payload.GetProperty("ignoreType").GetString()
                ?? throw new ArgumentNullException("ignoreType");

            switch (ignoreType)
            {
                case "file":
                    await _gitRunner.IgnoreFileAsync(repoPath, filePath);
                    break;

                case "folder":
                    await _gitRunner.IgnoreFolderAsync(repoPath, filePath);
                    break;

                case "extension":
                    await _gitRunner.IgnoreExtensionAsync(repoPath, filePath);
                    break;
            }

            // Response
            var response = new IpcMessage
            {
                Action = "CHANGE_IGNORED",
                Payload = JsonSerializer.SerializeToElement(new { repoPath = repoPath })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
