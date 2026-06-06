using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
    public interface IGitRunner
    {
        Task<string> ExecuteCommandAsync(string repoPath, string arguments);

        // Parsers
        Task<IEnumerable<GitBranch>> GetBranchesAsync(string repoPath);
        Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repoPath);
        Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int limit = 50);
        Task<string> GetFileDiffAsync(string repoPath, string filePath);
        Task<CommitDetailsResult> GetCommitDetailsAsync(string repoPath, string hash);

        // Commands
        Task CheckoutBranchAsync(string repoPath, string branchName, bool isRemote);
        Task CreateBranchAsync(string repoPath, string branchName);
        Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files);
        Task RenameBranchAsync(string repoPath, string oldName, string newName);
        Task DeleteBranchAsync(string repoPath, string branchName);
        Task FetchRepositoryAsync(string repoPath);
        Task DiscardChangeAsync(string repoPath, string filePath);
        Task IgnoreFileAsync(string repoPath, string filePath);
        Task IgnoreFolderAsync(string repoPath, string filePath);
        Task IgnoreExtensionAsync(string repoPath, string filePath);

        Task<GitRepository> CreateRepositoryAsync(string repoName, string localPath, string gitIgnore, string license);
    }
}
