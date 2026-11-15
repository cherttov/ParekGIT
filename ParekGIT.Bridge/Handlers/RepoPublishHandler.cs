using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoPublishHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitSyncService _syncService;

		public string Action => "REPO_PUBLISH";
		
		// Constructor
		public RepoPublishHandler(PhotinoWindow window, IGitSyncService syncService)
		{
			_window = window;
			_syncService = syncService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			// branchName not required as "IGitRunner.PushAsync()" always targets currently checked out branch
			await _syncService.PushAsync(repoPath);

			// Response
			var response = IpcMessage.Create("REPO_PUBLISHED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
