using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoPullHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitSyncService _syncService;

		public string Action => "REPO_PULL";

		// Constructor
		public RepoPullHandler(PhotinoWindow window, IGitSyncService syncService)
		{
			_window = window;
			_syncService = syncService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			await _syncService.PullAsync(repoPath);

			// Response
			var response = IpcMessage.Create("REPO_PULLED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
