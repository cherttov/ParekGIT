using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class StashListHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "GET_STASHES";

		// Constructor
		public StashListHandler(PhotinoWindow window, IGitRunner gitRunner)
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
				Action = "STASHES_LOADED",
				Payload = JsonSerializer.SerializeToElement(stashes)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
