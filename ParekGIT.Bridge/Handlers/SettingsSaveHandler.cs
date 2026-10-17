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

			UserSettings existingSettings = await _jsonStore.GetUserSettingsAsync();

			existingSettings.Theme = incomingSettings.Theme;

			await _jsonStore.SaveUserSettingsAsync(existingSettings);

			// Response
			var response = new IpcMessage
			{
				Action = "SETTINGS_SAVED",
				Payload = JsonSerializer.SerializeToElement(existingSettings)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
