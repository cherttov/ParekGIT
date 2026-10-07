using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class HistoryRevertHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly LiteDbStore _dbStore;
		private readonly IGitRunner _gitRunner;
		private readonly IRemoteSyncNotifier _syncNotifier;

		public string Action => "HISTORY_REVERT";

		// Constructor
		public HistoryRevertHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner, IRemoteSyncNotifier syncNotifier)
		{
			_window = window;
			_dbStore = dbStore;
			_gitRunner = gitRunner;
			_syncNotifier = syncNotifier;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string commitHash = payload.GetProperty("commitHash").GetString()
							  ?? throw new IpcPayloadException("commitHash");

			await _gitRunner.RevertCommitAsync(repoPath, commitHash);

			// Refresh remote sync status
			GitRepository repo = await _dbStore.GetRepositoryByPathAsync(repoPath)
				?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

			if (!string.IsNullOrEmpty(repo.RemoteUrl))
			{
				await _gitRunner.FetchRepositoryAsync(repoPath);

				int commitsBehind = await _gitRunner.GetCommitsBehindAsync(repoPath);
				int commitsAhead = await _gitRunner.GetCommitsAheadAsync(repoPath);
				_syncNotifier.NotifyCommitsBehind(repoPath, commitsBehind, commitsAhead);
			}

			// Response
			var response = new IpcMessage
			{
				Action = "HISTORY_REVERT_RESULT",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
