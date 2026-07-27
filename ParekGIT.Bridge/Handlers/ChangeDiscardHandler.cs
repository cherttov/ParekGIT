using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ChangeDiscardHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "CHANGE_DISCARD";

		// Constructor
		public ChangeDiscardHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string filePath = payload.GetProperty("filePath").GetString()
				?? throw new IpcPayloadException("filePath");

			await _gitRunner.DiscardChangeAsync(repoPath, filePath);

			// Response
			var response = new IpcMessage
			{
				Action = "CHANGE_DISCARDED",
				Payload = JsonSerializer.SerializeToElement("")
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
