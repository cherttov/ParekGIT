using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitHistoryService : IGitHistoryService
	{
		private readonly GitCommandExecutor _executor;
		private readonly ILogger _logger;
		private readonly IGitStatusService _status;

		// Constructor
		public GitHistoryService(GitCommandExecutor executor, ILogger logger, IGitStatusService status)
		{
			_executor = executor;
			_logger = logger;
			_status = status;
		}

		// Public methods
		public async Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int skip = 0, int take = 50) =>
			GitHistoryParser.Parse(await _executor.ExecuteAsync(repoPath, [
				"log", branchName, "--skip", skip.ToString(), "-n", take.ToString(), "--pretty=format:%H|%s|%an|%ar|%at"
			]));

		public async Task<CommitDetailsResult> GetCommitDetailsAsync(string repoPath, string hash) =>
			GitCommitDetailsParser.Parse(await _executor.ExecuteAsync(repoPath, [
				"show", "--diff-merges=first-parent", "--name-status", "--format=%an%n%s", hash
			]));

		public async Task CheckoutCommitAsync(string repoPath, string commitHash)
		{
			await _status.EnsureCleanWorkingTreeAsync(repoPath);
			await _executor.ExecuteAsync(repoPath, ["checkout", "--detach", commitHash]);
		}

		public async Task RevertCommitAsync(string repoPath, string commitHash)
		{
			await _status.EnsureCleanWorkingTreeAsync(repoPath);

			try
			{
				// Revert
				await _executor.ExecuteAsync(repoPath, ["revert", "--no-edit", commitHash]);
			}
			catch (Exception ex) when (
				ex.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
				ex.Message.Contains("could not revert", StringComparison.OrdinalIgnoreCase))
			{
				// Merge conflict
				await _logger.LogWarningAsync($"Revert conflict on commit '{commitHash}' in '{repoPath}'. Aborting. Git message: {ex.Message}");

				await _executor.ExecuteAsync(repoPath, ["revert", "--abort"]);

				throw new Exception("Merge conflict while reverting changes [RevertCommitAsync]");
			}
		}
	}
}
