using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using Photino.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var settings = payload.Deserialize<UserSettings>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new ArgumentNullException("settings");

            await _dbStore.SaveUserSettingsAsync(settings);

            // Response
            var response = new IpcMessage
            {
                Action = "SETTINGS_SAVED",
                Payload = JsonSerializer.SerializeToElement(settings)
            };
            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }
    }
}
