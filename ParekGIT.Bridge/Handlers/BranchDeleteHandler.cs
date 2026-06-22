using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class BranchDeleteHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IGitRunner _gitRunner;

        public string Action => "BRANCH_DELETE";

        // Constructor
        public BranchDeleteHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                              ?? throw new IpcPayloadException("repoPath");

            string branchName = payload.GetProperty("branchName").GetString()
                              ?? throw new IpcPayloadException("branchName");

            await _gitRunner.DeleteBranchAsync(repoPath, branchName);

            var responsePayload = new
            {
                repoPath = repoPath,
                branchName = branchName
            };

            // Response
            var response = new IpcMessage
            {
                Action = "BRANCH_DELETED",
                Payload = JsonSerializer.SerializeToElement(responsePayload)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
