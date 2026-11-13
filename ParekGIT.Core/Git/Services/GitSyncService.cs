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
		public async Task PushAsync(string repoPath, CancellationToken cancellationToken = default) =>
			await _executor.ExecuteAsync(repoPath, ["push", "-u", "origin", "HEAD"], cancellationToken);

		public async Task PullAsync(string repoPath, CancellationToken cancellationToken = default) =>
			await _executor.ExecuteAsync(repoPath, ["pull"], cancellationToken);

		public async Task FetchRepositoryAsync(string repoPath, CancellationToken cancellationToken = default) =>
			await _executor.ExecuteAsync(repoPath, ["fetch", "--all", "--prune"], cancellationToken);
	}
}
