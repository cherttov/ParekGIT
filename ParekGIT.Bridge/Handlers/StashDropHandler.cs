using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class StashDropHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitStashService _stashService;

		public string Action => "STASH_DROP";

		// Constructor
		public StashDropHandler(PhotinoWindow window, IGitStashService stashService)
		{
			_window = window;
			_stashService = stashService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string selector = payload.GetProperty("selector").GetString()
				?? throw new IpcPayloadException("selector");

			await _stashService.StashDropAsync(repoPath, selector);

			// Response
			var response = IpcMessage.Create("STASH_DROPPED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
