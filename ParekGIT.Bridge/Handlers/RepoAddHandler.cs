using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Core.Helpers;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoAddHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _repoStore;
		private readonly IGitStatusService _statusService;
		private readonly IFileSystemService _fileSystem;

		public string Action => "REPO_ADD";

		// Constructor
		public RepoAddHandler(PhotinoWindow window, IRepositoryStore repoStore, IGitStatusService statusService, IFileSystemService fileSystem)
		{
			_window = window;
			_repoStore = repoStore;
			_statusService = statusService;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			// Normalize path
			repoPath = PathHelper.Normalize(repoPath);
			if (repoPath.EndsWith(".git", PathHelper.Comparison))
				repoPath = PathHelper.Normalize(Path.GetDirectoryName(repoPath) ?? repoPath);

			// Verify if valid .git repo
			string gitFolderPath = Path.Combine(repoPath, ".git");
			if (!_fileSystem.DirectoryExists(gitFolderPath))
				throw new DirectoryNotFoundException("Selected folder is not a valid Git repository.");

			// Create repo object & check for remote
			string repoName = new DirectoryInfo(repoPath).Name;
			DateTime lastAccessed = new DirectoryInfo(repoPath).LastAccessTimeUtc;
			string? remoteUrl = await _statusService.GetRemoteUrlAsync(repoPath);

			var repo = new GitRepository
			{
				Id = Guid.NewGuid(),
				Name = repoName,
				AbsolutePath = repoPath,
				LastAccessed = lastAccessed,
				RemoteUrl = remoteUrl
			};

			if (await _repoStore.GetRepositoryAsync(repoPath) != null)
				throw new InvalidOperationException("Repository with this path already exists in the database.");

			// Save to db
			await _repoStore.UpsertRepositoryAsync(repo);

			// Response
			var response = IpcMessage.Create("REPO_ADDED", new { repo });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
