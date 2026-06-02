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
    public class HistoryFileDiffHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly GitCliRunner _gitRunner;

        public string Action => "GET_HISTORY_FILE_DIFF";

        public HistoryFileDiffHandler(PhotinoWindow window, GitCliRunner gitRunner)
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

            string filePath = payload.GetProperty("filePath").GetString()
                              ?? throw new ArgumentNullException("filePath");

            string diffText = await _gitRunner.GetHistoryFileDiffAsync(repoPath, commitHash, filePath);

            // Response
            var response = new IpcMessage
            {
                Action = "HISTORY_FILE_DIFF_LOADED",
                Payload = JsonSerializer.SerializeToElement(new { diffText = diffText })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
