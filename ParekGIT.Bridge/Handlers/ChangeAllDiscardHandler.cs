using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ChangeAllDiscardHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "CHANGE_ALL_DISCARD";

		// Constructor
		public ChangeAllDiscardHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			await _gitRunner.DiscardAllChangesAsync(repoPath);

			// Response
			var response = new IpcMessage
			{
				Action = "CHANGE_DISCARDED",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
