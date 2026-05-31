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
    public class RepoFetchHandler : IMessageHandler
    {
        private PhotinoWindow _window;
        private IGitRunner _gitRunner;

        public string Action => "REPO_FETCH";

        public RepoFetchHandler(PhotinoWindow window, IGitRunner gitRunner)
        {
            _window = window;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            await _gitRunner.FetchRepositoryAsync(repoPath);

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_FETCHED",
                Payload = JsonSerializer.SerializeToElement(new { status = "SUCCESS" })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
