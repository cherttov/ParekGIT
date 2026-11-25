using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoFetchHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitSyncService _syncStatus;
		private readonly IGitStatusService _statusService;
		private readonly IRemoteSyncNotifier _syncNotifier;

		public string Action => "REPO_FETCH";

		// Constructor
		public RepoFetchHandler(PhotinoWindow window, IGitSyncService syncStatus, IGitStatusService statusService, IRemoteSyncNotifier syncNotifier)
		{
			_window = window;
			_syncStatus = syncStatus;
			_statusService = statusService;
			_syncNotifier = syncNotifier;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			await _syncStatus.FetchRepositoryAsync(repoPath);

			int commitsBehind = await _statusService.GetCommitsBehindAsync(repoPath);
			int commitsAhead = await _statusService.GetCommitsAheadAsync(repoPath);
			_syncNotifier.NotifyCommitsBehind(repoPath, commitsBehind, commitsAhead);

			// Response
			var response = IpcMessage.Create("REPO_FETCHED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
