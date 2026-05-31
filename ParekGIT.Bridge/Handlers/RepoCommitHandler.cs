using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoCommitHandler : IMessageHandler
    {
        private PhotinoWindow _window;
        private IGitRunner _gitRunner;

        public string Action => "REPO_COMMIT";

        // Constructor
        public RepoCommitHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            string message = payload.GetProperty("message").GetString()
                ?? throw new ArgumentNullException("message");

            string description = payload.GetProperty("description").GetString()
                ?? throw new ArgumentNullException("description");

            var files = new List<string>();
            foreach (var file in payload.GetProperty("files").EnumerateArray())
            {
                files.Add(file.GetString() ?? "");
            }
            if (files.Count == 0) { throw new ArgumentNullException("files"); }

            // Run commands (.Core)
            await _gitRunner.CommitAsync(repoPath, message, description, files);

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_COMMITTED",
                Payload = JsonSerializer.SerializeToElement("")
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
