using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Services;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoWatcherHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;

        public string Action => "INTERNAL_WATCHER_EVENTS";

        // Constructor
        public RepoWatcherHandler(PhotinoWindow window, RepoWatcher repoWatcher)
        {
            _window = window;
            repoWatcher.OnFilesChanged += HandleFilesChanged;
        }

        // Not used, as it's internal handler
        public Task ExecuteAsync(JsonElement payload)
        {
            return Task.CompletedTask;
        }

        // Instead of ExecuteAsync
        private void HandleFilesChanged(object? sender, string repoPath)
        {
            var envelope = new
            {
                Action = "REPO_FILES_CHANGED",
                Payload = new { repoPath = repoPath }
            };

            string jsonMessage = JsonSerializer.Serialize(envelope);
            _window.SendWebMessage(jsonMessage);
        }
    }
}
