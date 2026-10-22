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
		private readonly IGitRunner _gitRunner;

		public string Action => "STASH_LIST_LOAD";

		// Constructor
		public StashListLoadHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			IEnumerable<GitStash> stashes = await _gitRunner.GetStashesAsync(repoPath);

			// Response
			var response = new IpcMessage
			{
				Action = "STASH_LIST_LOADED",
				Payload = JsonSerializer.SerializeToElement(stashes)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
