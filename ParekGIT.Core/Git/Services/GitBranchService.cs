using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitBranchService : IGitBranchService
	{
		private readonly GitCommandExecutor _executor;
		private readonly ILogger _logger;
		private readonly IGitStatusService _status;

		// Constructor
		public GitBranchService(GitCommandExecutor executor, ILogger logger, IGitStatusService status)
		{
			_executor = executor;
			_logger = logger;
			_status = status;
		}

		// Public methods
		public async Task<IEnumerable<GitBranch>> GetBranchesAsync(string repoPath) =>
			GitBranchParser.Parse(await _executor.ExecuteAsync(repoPath, [
				"branch", "--all", "--format=%(refname)|%(refname:short)|%(HEAD)|%(upstream:short)|%(objectname)"
			]));

		public async Task CheckoutBranchAsync(string repoPath, string branchName, bool isRemote)
		{
			string[] args = isRemote
				? ["checkout", "-t", branchName]
				: ["checkout", branchName];

			await _executor.ExecuteAsync(repoPath, args);
		}

		public async Task CreateBranchAsync(string repoPath, string branchName) =>
			await _executor.ExecuteAsync(repoPath, ["checkout", "-b", branchName]);

		public async Task CreateBranchFromCommitAsync(string repoPath, string branchName, string commitHash) =>
			await _executor.ExecuteAsync(repoPath, ["checkout", "-b", branchName, commitHash]);

		public async Task RenameBranchAsync(string repoPath, string oldName, string newName) =>
			await _executor.ExecuteAsync(repoPath, ["branch", "-m", oldName, newName]);

		public async Task DeleteBranchAsync(string repoPath, string branchName) =>
			await _executor.ExecuteAsync(repoPath, ["branch", "-d", branchName]);

		public async Task ForceDeleteBranchAsync(string repoPath, string branchName) =>
			await _executor.ExecuteAsync(repoPath, ["branch", "-D", branchName]);

		public async Task DeleteRemoteBranchAsync(string repoPath, string remoteName, string branchName) =>
			await _executor.ExecuteAsync(repoPath, ["push", remoteName, "-d", branchName]);

		public async Task MergeBranchesAsync(string repoPath, string sourceBranch, string targetBranch)
		{
			await _status.EnsureCleanWorkingTreeAsync(repoPath);
			await _executor.ExecuteAsync(repoPath, ["checkout", targetBranch]);

			try
			{
				await _executor.ExecuteAsync(repoPath, ["merge", sourceBranch]);
			}
			catch (Exception ex) when (
				ex.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
				ex.Message.Contains("merge failed", StringComparison.OrdinalIgnoreCase))
			{
				await _logger.LogWarningAsync($"Merge conflict while merging '{sourceBranch}' -> '{targetBranch}'. Aborting. Git message: {ex.Message}");

				await _executor.ExecuteAsync(repoPath, ["merge", "--abort"]);

				throw new Exception($"Merge conflict occurred while merging '{sourceBranch}' -> '{targetBranch}'. Merge aborted.");
			}
		}
	}
}
