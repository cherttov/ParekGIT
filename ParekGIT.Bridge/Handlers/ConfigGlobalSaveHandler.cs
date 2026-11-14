using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ConfigGlobalSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitConfigService _configService;

		public string Action => "CONFIG_GLOBAL_SAVE";

		// Constructor
		public ConfigGlobalSaveHandler(PhotinoWindow window, IGitConfigService configService)
		{
			_window = window;
			_configService = configService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string name = payload.GetProperty("name").GetString()
							  ?? throw new IpcPayloadException("name");

			string email = payload.GetProperty("email").GetString()
							  ?? throw new IpcPayloadException("email");

			await _configService.SaveGlobalConfigAsync(null, name, email);

			// Response
			var response = IpcMessage.Create("CONFIG_GLOBAL_SAVED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
