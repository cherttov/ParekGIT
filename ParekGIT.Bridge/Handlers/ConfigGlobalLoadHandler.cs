using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ConfigGlobalLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitConfigService _configService;

		public string Action => "CONFIG_GLOBAL_LOAD";

		// Constructor
		public ConfigGlobalLoadHandler(PhotinoWindow window, IGitConfigService configService)
		{
			_window = window;
			_configService = configService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			GitConfigInfo globalConfig = await _configService.GetGlobalConfigAsync(null);

			// Response
			var response = IpcMessage.Create("CONFIG_GLOBAL_LOADED", new
			{
				globalName = globalConfig.Name,
				globalEmail = globalConfig.Email
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
