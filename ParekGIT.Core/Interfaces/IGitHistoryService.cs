using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitHistoryService
	{
		Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int skip = 0, int take = 50);
		Task<CommitDetailsResult> GetCommitDetailsAsync(string repoPath, string hash);
		Task CheckoutCommitAsync(string repoPath, string commitHash);
		Task RevertCommitAsync(string repoPath, string commitHash);
	}
}
