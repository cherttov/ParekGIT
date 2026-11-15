using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class StashListLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitStashService _stashService;

		public string Action => "STASH_LIST_LOAD";

		// Constructor
		public StashListLoadHandler(PhotinoWindow window, IGitStashService stashService)
		{
			_window = window;
			_stashService = stashService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			IEnumerable<GitStash> stashes = await _stashService.GetStashesAsync(repoPath);

			// Response
			var response = IpcMessage.Create("STASH_LIST_LOADED", new { stashes });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
