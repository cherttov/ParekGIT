using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoWatcherHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepoWatcher _repoWatcher;

		public string Action => "INTERNAL_WATCHER_EVENTS";

		// Constructor
		public RepoWatcherHandler(PhotinoWindow window, IRepoWatcher repoWatcher)
		{
			_window = window;
			_repoWatcher = repoWatcher;
			_repoWatcher.OnFilesChanged += HandleFilesChanged;
		}

		// Not used, as it's internal handler
		public Task ExecuteAsync(JsonElement payload)
		{
			return Task.CompletedTask;
		}

		// Instead of ExecuteAsync
		private void HandleFilesChanged(object? sender, string repoPath)
		{
			var envelope = IpcMessage.Create("REPO_FILES_CHANGED", new { repoPath });
			_window.SendWebMessage(JsonSerializer.Serialize(envelope));
		}
	}
}
