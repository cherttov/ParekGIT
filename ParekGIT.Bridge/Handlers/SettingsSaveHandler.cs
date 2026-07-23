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
		private readonly ISettingsStore _dbStore;

		public string Action => "SETTINGS_SAVE";

		// Constructor
		public SettingsSaveHandler(PhotinoWindow window, ISettingsStore dbStore)
		{
			_window = window;
			_dbStore = dbStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			UserSettings incomingSettings = payload.Deserialize<UserSettings>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
				?? throw new IpcPayloadException("settings");

			UserSettings existingSettings = await _dbStore.GetUserSettingsAsync();

			existingSettings.Theme = incomingSettings.Theme;

			await _dbStore.SaveUserSettingsAsync(existingSettings);

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
