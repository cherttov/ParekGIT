using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Core.Helpers;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoOpenHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _repoStore;
		private readonly ISettingsStore _settingsStore;
		private readonly IGitBranchService _branchService;
		private readonly IRepositoryWatcher _repoWatcher;
		private readonly IFileSystemService _fileSystem;

		public string Action => "REPO_OPEN";

		// Constructor
		public RepoOpenHandler(PhotinoWindow window, IRepositoryStore repoStore, ISettingsStore settingsStore,
			IGitBranchService branchService, IRepositoryWatcher repoWatcher, IFileSystemService fileSystem)
		{
			_window = window;
			_repoStore = repoStore;
			_settingsStore = settingsStore;
			_branchService = branchService;
			_repoWatcher = repoWatcher;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			var repo = await _repoStore.GetRepositoryAsync(repoPath);

			// Failed response
			if (!_fileSystem.DirectoryExists(repoPath) || repo == null)
			{
				await _repoStore.UpdateRepositoryValidStateAsync(repoPath, false);

				// Make an IPC message that path is missing to resolve
				var missingResponse = IpcMessage.Create("REPO_PATH_MISSING", new { repoPath });
				_window.SendWebMessage(JsonSerializer.Serialize(missingResponse));
				return;
			}

			// Update last accessed timestamp
			await _repoStore.TouchRepositoryAsync(repoPath);

			// Get branches
			IEnumerable<GitBranch> branches = await _branchService.GetBranchesAsync(repoPath);
			GitBranch branch = branches.FirstOrDefault(b => b.IsCurrent == true)!;
			_repoWatcher.WatchRepository(repoPath);

			// Update last repo path
			await _settingsStore.UpdateUserSettingsAsync(settings =>
			{
				if (PathHelper.AreEqual(settings.LastRepoPath, repoPath))
					return false;

				settings.LastRepoPath = repoPath;
				return true;
			});

			// Response
			var response = IpcMessage.Create("REPO_OPENED", new
			{
				repo,
				branch,
				branches
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
