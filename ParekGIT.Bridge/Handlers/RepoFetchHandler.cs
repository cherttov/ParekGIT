using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoFetchHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;
		private readonly IRemoteSyncNotifier _syncNotifier;

		public string Action => "REPO_FETCH";

		// Constructor
		public RepoFetchHandler(PhotinoWindow window, IGitRunner gitRunner, IRemoteSyncNotifier syncNotifier)
		{
			_window = window;
			_gitRunner = gitRunner;
			_syncNotifier = syncNotifier;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			await _gitRunner.FetchRepositoryAsync(repoPath);

			int commitsBehind = await _gitRunner.GetCommitsBehindAsync(repoPath);
			_syncNotifier.NotifyCommitsBehind(repoPath, commitsBehind);

			// Response
			var response = new IpcMessage
			{
				Action = "REPO_FETCHED",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
