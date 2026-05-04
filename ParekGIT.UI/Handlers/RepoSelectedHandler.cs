using ParekGIT.Core.Data;
using ParekGIT.UI.Interfaces;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParekGIT.UI.Handlers
{
    internal class RepoSelectedHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;

        public string Action => "REPO_SELECTED";

        public RepoSelectedHandler(PhotinoWindow window, LiteDbStore dbStore)
        {
            _window = window;
            _dbStore = dbStore;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("absolutePath").GetString();
            Console.WriteLine($"Selected repo: {repoPath}");

            await Task.CompletedTask;
        }
    }
}
