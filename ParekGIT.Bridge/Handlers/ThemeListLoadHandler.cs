using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ThemeListLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IThemeStore _themeStore;

		public string Action => "THEME_LIST_LOAD";

		// Constructor
		public ThemeListLoadHandler(PhotinoWindow window, IThemeStore themeStore)
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
				Action = "THEME_LIST_LOADED",
				Payload = JsonSerializer.SerializeToElement(themes)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
