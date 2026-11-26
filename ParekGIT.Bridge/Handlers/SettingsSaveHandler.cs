using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class SettingsSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly ISettingsStore _settingsStore;

		public string Action => "SETTINGS_SAVE";

		// Constructor
		public SettingsSaveHandler(PhotinoWindow window, ISettingsStore settingsStore)
		{
			_window = window;
			_settingsStore = settingsStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			UserSettings incomingSettings = payload.Deserialize<UserSettings>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
				?? throw new IpcPayloadException("settings");

			UserSettings settings = await _settingsStore.GetUserSettingsAsync();

			settings.Theme = incomingSettings.Theme;

			await _settingsStore.SaveUserSettingsAsync(settings);

			// Response
			var response = IpcMessage.Create("SETTINGS_SAVED", new { settings });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
