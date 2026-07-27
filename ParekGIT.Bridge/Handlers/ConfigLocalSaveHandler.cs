using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ConfigLocalSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "CONFIG_LOCAL_SAVE";

		// Constructor
		public ConfigLocalSaveHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string name = payload.GetProperty("name").GetString()
							  ?? throw new IpcPayloadException("name");

			string email = payload.GetProperty("email").GetString()
							  ?? throw new IpcPayloadException("email");

			await _gitRunner.SaveLocalConfigAsync(repoPath, name, email);

			// Response
			var response = new IpcMessage
			{
				Action = "CONFIG_LOCAL_SAVED",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
