using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ThemesListhandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IThemeStore _themeStore;

		public string Action => "GET_THEMES";

		// Constructor
		public ThemesListhandler(PhotinoWindow window, IThemeStore themeStore)
		{
			_window = window;
			_themeStore = themeStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			var themes = await _themeStore.LoadThemes();

			// Response
			var response = new IpcMessage
			{
				Action = "THEMES_LOADED",
				Payload = JsonSerializer.SerializeToElement(themes)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
