using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoAddHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _dbStore;
		private readonly IGitStatusService _statusService;
		private readonly IFileSystemService _fileSystem;

		public string Action => "REPO_ADD";

		// Constructor
		public RepoAddHandler(PhotinoWindow window, IRepositoryStore dbStore, IGitStatusService statusService, IFileSystemService fileSystem)
		{
			_window = window;
			_dbStore = dbStore;
			_statusService = statusService;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			// Normalize path
			repoPath = Path.GetFullPath(repoPath);
			repoPath = Path.TrimEndingDirectorySeparator(repoPath);
			if (repoPath.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
				repoPath = Path.GetDirectoryName(repoPath) ?? repoPath;

			// Verify if valid .git repo
			string gitFolderPath = Path.Combine(repoPath, ".git");
			if (!_fileSystem.DirectoryExists(gitFolderPath))
				throw new DirectoryNotFoundException("Selected folder is not a valid Git repository.");

			// Create repo object
			string repoName = new DirectoryInfo(repoPath).Name;
			DateTime lastAccessed = new DirectoryInfo(repoPath).LastAccessTime;

			// Check for remote
			string? remoteUrl = await _statusService.GetRemoteUrlAsync(repoPath);

			var repo = new GitRepository
			{
				Id = Guid.NewGuid(),
				Name = repoName,
				AbsolutePath = repoPath,
				LastAccessed = lastAccessed,
				RemoteUrl = remoteUrl
			};

			IEnumerable<GitRepository> allRepos = await _dbStore.GetAllRepositoriesAsync();
			if (allRepos.Any(existing => string.Equals(existing.AbsolutePath, repoPath, StringComparison.OrdinalIgnoreCase)))
				throw new InvalidOperationException("Repository with this path already exists in the database.");

			// Save to db
			await _dbStore.UpsertRepositoryAsync(repo);

			// Response
			var response = IpcMessage.Create("REPO_ADDED", new { repo });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
