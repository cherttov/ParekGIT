using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class AppReadyHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _repoStore;
		private readonly ISettingsStore _settingsStore;
		private readonly IThemeStore _themeStore;
		private readonly IFileSystemService _fileSystem;

		public string Action => "APP_READY";

		// Constructor
		public AppReadyHandler(PhotinoWindow window, IRepositoryStore repoStore,
			ISettingsStore settingsStore, IThemeStore themeStore, IFileSystemService fileSystem)
		{
			_window = window;
			_repoStore = repoStore;
			_settingsStore = settingsStore;
			_themeStore = themeStore;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			// Repos
			IEnumerable<GitRepository> repositories = await _repoStore.GetAllRepositoriesAsync();

			// Check if invalid repos are still invalid
			foreach (GitRepository repo in repositories)
			{
				bool isValidNow = _fileSystem.DirectoryExists(repo.AbsolutePath);

				if (repo.IsValid != isValidNow)
				{
					repo.IsValid = isValidNow;
					await _repoStore.UpsertRepositoryAsync(repo);
				}
			}

			// Settings
			UserSettings settings = await _settingsStore.GetUserSettingsAsync();

			// Themes
			var themes = await _themeStore.LoadThemesAsync();

			// Response
			var response = IpcMessage.Create("APP_INITIALIZED", new
			{
				repositories,
				settings,
				themes
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
