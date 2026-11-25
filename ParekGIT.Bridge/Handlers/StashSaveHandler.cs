using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class StashSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitStashService _stashService;

		public string Action => "STASH_SAVE";

		// Constructor
		public StashSaveHandler(PhotinoWindow window, IGitStashService stashService)
		{
			_window = window;
			_stashService = stashService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string message = payload.GetProperty("message").GetString()
				?? "";

			List<string> files = payload.GetProperty("files")
					.EnumerateArray()
					.Select(f => f.GetString() ?? string.Empty)
					.Where(f => f.Length > 0)
					.ToList()
				?? throw new IpcPayloadException("files");

			await _stashService.StashSaveAsync(repoPath, message, files);

			// Response
			var response = IpcMessage.Create("STASH_SAVED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
