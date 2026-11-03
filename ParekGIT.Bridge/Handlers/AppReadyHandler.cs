using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class AppReadyHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly LiteDbStore _dbStore;
		private readonly JsonStore _jsonStore;
		private readonly IFileSystemService _fileSystem;

		public string Action => "APP_READY";

		// Constructor
		public AppReadyHandler(PhotinoWindow window, LiteDbStore dbStore, JsonStore jsonStore, IFileSystemService fileSystem)
		{
			_window = window;
			_dbStore = dbStore;
			_jsonStore = jsonStore;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			// Repos
			IEnumerable<GitRepository> repositories = await _dbStore.GetAllRepositoriesAsync();

			// Check if invalid repos are still invalid
			foreach (GitRepository repo in repositories)
			{
				bool isValidNow = _fileSystem.DirectoryExists(repo.AbsolutePath);

				if (repo.IsValid != isValidNow)
				{
					repo.IsValid = isValidNow;
					await _dbStore.UpsertRepositoryAsync(repo);
				}
			}

			// Settings
			UserSettings settings = await _jsonStore.GetUserSettingsAsync();

			// Themes
			var themes = await _jsonStore.LoadThemes();

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
