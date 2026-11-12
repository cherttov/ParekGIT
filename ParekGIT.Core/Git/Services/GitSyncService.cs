using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitSyncService : IGitSyncService
	{
		private readonly GitCommandExecutor _executor;

		// Constructor
		public GitSyncService(GitCommandExecutor executor)
		{
			_executor = executor;
		}

		// Public methods
		public async Task PushAsync(string repoPath) =>
			await _executor.ExecuteAsync(repoPath, ["push", "-u", "origin", "HEAD"]);

		public async Task PullAsync(string repoPath) =>
			await _executor.ExecuteAsync(repoPath, ["pull"]);

		public async Task FetchRepositoryAsync(string repoPath) =>
			await _executor.ExecuteAsync(repoPath, ["fetch", "--all", "--prune"]);
	}
}
