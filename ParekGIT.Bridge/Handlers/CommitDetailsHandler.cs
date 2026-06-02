using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Git;
using ParekGIT.Core.Models;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParekGIT.Bridge.Handlers
{
    public class CommitDetailsHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly GitCliRunner _gitRunner;

        public string Action => "GET_COMMIT_DETAILS";

        // Constructor
        public CommitDetailsHandler(PhotinoWindow window, GitCliRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                              ?? throw new ArgumentNullException("repoPath");

            string commitHash = payload.GetProperty("commitHash").GetString()
                              ?? throw new ArgumentNullException("commitHash");

            CommitDetailsResult details = await _gitRunner.GetCommitDetailsAsync(repoPath, commitHash);

            // Convert to json payload
            var jsPayload = new
            {
                author = details.Author,
                fileCount = details.Files.Count(),
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
