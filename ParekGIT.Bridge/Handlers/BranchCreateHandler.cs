using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class BranchCreateHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IGitRunner _gitRunner;

        public string Action => "BRANCH_CREATE";

        // Constructor
        public BranchCreateHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("absolutePath").GetString()
                              ?? throw new ArgumentNullException("absolutePath");

            string branchName = payload.GetProperty("branchName").GetString()
                                ?? throw new ArgumentNullException("branchName");

            // Create new branch
            await _gitRunner.CreateBranchAsync(repoPath, branchName);

            // Update displayed list
            var branches = await _gitRunner.GetBranchesAsync(repoPath);

            // Response
            var response = new IpcMessage
            {
                Action = "BRANCH_LOADED", // since it updates the dropdown just use this or "BRANCHES_LOADED"
                Payload = JsonSerializer.SerializeToElement(branches)
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
