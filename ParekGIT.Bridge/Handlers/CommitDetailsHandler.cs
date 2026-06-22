using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class CommitDetailsHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IGitRunner _gitRunner;

        public string Action => "GET_COMMIT_DETAILS";

        // Constructor
        public CommitDetailsHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                              ?? throw new IpcPayloadException("repoPath");

            string commitHash = payload.GetProperty("commitHash").GetString()
                              ?? throw new IpcPayloadException("commitHash");

            CommitDetailsResult details = await _gitRunner.GetCommitDetailsAsync(repoPath, commitHash);

            // Convert to json payload
            var jsPayload = new
            {
                author = details.Author,
                message = details.Message,
                files = details.Files
            };

            // Response
            var response = new IpcMessage
            {
                Action = "COMMIT_DETAILS_LOADED",
                Payload = JsonSerializer.SerializeToElement(jsPayload)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
