using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ChangesCommitHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _dbStore;
		private readonly IGitWorkingTreeService _workingTreeService;
		private readonly IGitSyncService _syncService;
		private readonly IGitStatusService _statusService;
		private readonly IRemoteSyncNotifier _syncNotifier;

		public string Action => "CHANGES_COMMIT";

		// Constructor
		public ChangesCommitHandler(PhotinoWindow window, IRepositoryStore dbStore, IGitWorkingTreeService workingTreeService,
			IGitSyncService syncService, IGitStatusService statusService, IRemoteSyncNotifier syncNotifier)
		{
			_window = window;
			_workingTreeService = workingTreeService;
			_syncService = syncService;
			_statusService = statusService;
			_dbStore = dbStore;
			_syncNotifier = syncNotifier;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string message = payload.GetProperty("message").GetString()
				?? throw new IpcPayloadException("message");

			string description = payload.TryGetProperty("description", out var descriptionProperty)
				? (descriptionProperty.GetString() ?? "")
				: "";

			var files = new List<string>();
			foreach (var file in payload.GetProperty("files").EnumerateArray())
				files.Add(file.GetString() ?? "");
			if (files.Count == 0) throw new IpcPayloadException("files", "must contain at least 1 file");

			// Commit locally
			await _workingTreeService.CommitAsync(repoPath, message, description, files);

			// Refresh remote sync status
			GitRepository repo = await _dbStore.GetRepositoryByPathAsync(repoPath)
				?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

			if (!string.IsNullOrEmpty(repo.RemoteUrl))
			{
				await _syncService.FetchRepositoryAsync(repoPath);

				int commitsBehind = await _statusService.GetCommitsBehindAsync(repoPath);
				int commitsAhead = await _statusService.GetCommitsAheadAsync(repoPath);
				_syncNotifier.NotifyCommitsBehind(repoPath, commitsBehind, commitsAhead);
			}

			// Response
			var response = IpcMessage.Create("CHANGES_COMMITTED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
