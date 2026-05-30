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
                              ?? throw new ArgumentNullException("repoPath");

            string branchName = payload.GetProperty("branchName").GetString()
                              ?? throw new ArgumentNullException("branchName");

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
