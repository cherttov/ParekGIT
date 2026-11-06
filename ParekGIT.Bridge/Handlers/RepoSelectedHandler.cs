using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoSelectedHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly LiteDbStore _dbStore;
		private readonly JsonStore _jsonStore;
		private readonly IGitRunner _gitRunner;
		private readonly IRepoWatcher _repoWatcher;
		private readonly IFileSystemService _fileSystem;

		public string Action => "REPO_SELECTED";

		// Constructor
		public RepoSelectedHandler(PhotinoWindow window, LiteDbStore dbStore, JsonStore jsonStore, IGitRunner gitRunner, IRepoWatcher repoWatcher, IFileSystemService fileSystem)
		{
			_window = window;
			_dbStore = dbStore;
			_jsonStore = jsonStore;
			_gitRunner = gitRunner;
			_repoWatcher = repoWatcher;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			// Failed response
			if (!_fileSystem.DirectoryExists(repoPath))
			{
				await _dbStore.UpdateRepositoryValidStateAsync(repoPath, false);

				// Make an IPC message that path is missing to resolve
				var missingResponse = IpcMessage.Create("REPO_PATH_MISSING", new { repoPath });
				_window.SendWebMessage(JsonSerializer.Serialize(missingResponse));
				return;
			}

			UserSettings currentSettings = await _jsonStore.GetUserSettingsAsync();

			// Get branches
			IEnumerable<GitBranch> branches = await _gitRunner.GetBranchesAsync(repoPath);
			_repoWatcher.WatchRepository(repoPath);

			// Update last repo path
			if (currentSettings.LastRepoPath != repoPath)
			{
				currentSettings.LastRepoPath = repoPath;
				await _jsonStore.SaveUserSettingsAsync(currentSettings);
			}

			// Response
			var response = IpcMessage.Create("BRANCH_LIST_LOADED", new { branches });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
