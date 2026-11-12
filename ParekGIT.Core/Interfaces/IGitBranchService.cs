using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitBranchService
	{
		Task<IEnumerable<GitBranch>> GetBranchesAsync(string repoPath);
		Task CheckoutBranchAsync(string repoPath, string branchName, bool isRemote);
		Task CreateBranchAsync(string repoPath, string branchName);
		Task CreateBranchFromCommitAsync(string repoPath, string branchName, string commitHash);
		Task RenameBranchAsync(string repoPath, string oldName, string newName);
		Task DeleteBranchAsync(string repoPath, string branchName);
		Task ForceDeleteBranchAsync(string repoPath, string branchName);
		Task DeleteRemoteBranchAsync(string repoPath, string remoteName, string branchName);
		Task MergeBranchesAsync(string repoPath, string sourceBranch, string targetBranch);
	}
}
