using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class SettingsSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly ISettingsStore _jsonStore;

		public string Action => "SETTINGS_SAVE";

		// Constructor
		public SettingsSaveHandler(PhotinoWindow window, ISettingsStore jsonStore)
		{
			_window = window;
			_jsonStore = jsonStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			UserSettings incomingSettings = payload.Deserialize<UserSettings>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
				?? throw new IpcPayloadException("settings");

			UserSettings settings = await _jsonStore.GetUserSettingsAsync();

			settings.Theme = incomingSettings.Theme;

			await _jsonStore.SaveUserSettingsAsync(settings);

			// Response
			var response = IpcMessage.Create("SETTINGS_SAVED", new { settings });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
