using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoCommitHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IGitRunner _gitRunner;

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
                ?? throw new IpcPayloadException("repoPath");

            string message = payload.GetProperty("message").GetString()
                ?? throw new IpcPayloadException("message");

            string description = payload.TryGetProperty("description", out var descriptionProperty)
                ? (descriptionProperty.GetString() ?? "")
                : "";

            var files = new List<string>();
            foreach (var file in payload.GetProperty("files").EnumerateArray())
            {
                files.Add(file.GetString() ?? "");
            }
            if (files.Count == 0) { throw new IpcPayloadException("files", "must contain at least 1 file"); }

            // Run commands (.Core)
            await _gitRunner.CommitAsync(repoPath, message, description, files);

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_COMMITTED",
                Payload = JsonSerializer.SerializeToElement(new { success = true })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
