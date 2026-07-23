using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitRunner
	{
		Task<string> ExecuteCommandAsync(string? repoPath, string arguments, params int[] successExitCodes);

		// Parsers
		Task<IEnumerable<GitBranch>> GetBranchesAsync(string repoPath);
		Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repoPath);
		Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int skip = 0, int take = 50);
		Task<string> GetFileDiffAsync(string repoPath, string filePath);
		Task<CommitDetailsResult> GetCommitDetailsAsync(string repoPath, string hash);
		Task<int> GetCommitsBehindAsync(string repoPath);
		Task<string> GetHistoryFileDiffAsync(string repoPath, string commitHash, string filePath);
		Task<GitConfigInfo> GetGlobalConfigAsync(string? repoPath);
		Task<GitConfigInfo> GetLocalConfigAsync(string repoPath);

		// Commands
		Task CheckoutBranchAsync(string repoPath, string branchName, bool isRemote);
		Task CreateBranchAsync(string repoPath, string branchName);
		Task CreateBranchFromCommitAsync(string repoPath, string branchName, string commitHash);
		Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files);
		Task PushAsync(string repoPath);
		Task PullAsync(string repoPath);
		Task RenameBranchAsync(string repoPath, string oldName, string newName);
		Task DeleteBranchAsync(string repoPath, string branchName);
		Task FetchRepositoryAsync(string repoPath);
		Task DiscardChangeAsync(string repoPath, string filePath);
		Task IgnoreFileAsync(string repoPath, string filePath);
		Task IgnoreFolderAsync(string repoPath, string filePath);
		Task IgnoreExtensionAsync(string repoPath, string filePath);
		Task CheckoutCommitAsync(string repoPath, string commitHash);
		Task RevertCommitAsync(string repoPath, string commitHash);
		Task MergeBranchesAsync(string repoPath, string sourceBranch, string targetBranch);
		Task<GitRepository> CreateRepositoryAsync(CreateRepoRequest request);
		Task CloneRepositoryAsync(string repoUrl, bool asLocal, string localPath);
		Task SaveLocalConfigAsync(string repoPath, string name, string email);
		Task SaveGlobalConfigAsync(string? repoPath, string name, string email);
	}
}
