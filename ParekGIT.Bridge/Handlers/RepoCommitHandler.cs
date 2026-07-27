using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoCommitHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _dbStore;
		private readonly IGitRunner _gitRunner;
		private readonly IRemoteSyncNotifier _syncNotifier;

		public string Action => "REPO_COMMIT";

		// Constructor
		public RepoCommitHandler(PhotinoWindow window, IRepositoryStore dbStore, IGitRunner gitRunner, IRemoteSyncNotifier syncNotifier)
		{
			_window = window;
			_gitRunner = gitRunner;
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
			{
				files.Add(file.GetString() ?? "");
			}
			if (files.Count == 0) { throw new IpcPayloadException("files", "must contain at least 1 file"); }

			// Commit locally
			await _gitRunner.CommitAsync(repoPath, message, description, files);

			// Remote push if not remote
			GitRepository? repo = await _dbStore.GetRepositoryByPathAsync(repoPath);
			bool pushed = false;

			if (repo?.IsRemote == true)
			{
				await _gitRunner.FetchRepositoryAsync(repoPath);
				int commitsBehind = await _gitRunner.GetCommitsBehindAsync(repoPath);
				_syncNotifier.NotifyCommitsBehind(repoPath, commitsBehind);

				if (commitsBehind == 0)
				{
					await _gitRunner.PushAsync(repoPath);
					pushed = true;
				}
			}

			// Response
			var response = new IpcMessage
			{
				Action = "REPO_COMMITTED",
				Payload = JsonSerializer.SerializeToElement(new { success = true, pushed })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
