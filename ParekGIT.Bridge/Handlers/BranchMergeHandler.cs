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
    public class BranchMergeHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly IGitRunner _gitRunner;

        public string Action => "BRANCH_MERGE";

        // Constructor
        public BranchMergeHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                              ?? throw new ArgumentNullException("repoPath");

            string sourceBranch = payload.GetProperty("sourceBranch").GetString()
                              ?? throw new ArgumentNullException("sourceBranch");

            string targetBranch = payload.GetProperty("targetBranch").GetString()
                              ?? throw new ArgumentNullException("targetBranch");

            await _gitRunner.MergeBranchesAsync(repoPath, sourceBranch, targetBranch);

            // Response
            var response = new IpcMessage
            {
                Action = "BRANCH_MERGED",
                Payload = JsonSerializer.SerializeToElement("")
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
