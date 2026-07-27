using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ConfigGlobalGetHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "CONFIG_GLOBAL_GET";

		// Constructor
		public ConfigGlobalGetHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			GitConfigInfo globalConfig = await _gitRunner.GetGlobalConfigAsync(null);

			var jsPayload = new
			{
				globalName = globalConfig.Name,
				globalEmail = globalConfig.Email
			};

			// Response
			var response = new IpcMessage
			{
				Action = "CONFIG_GLOBAL_LOADED",
				Payload = JsonSerializer.SerializeToElement(jsPayload)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
