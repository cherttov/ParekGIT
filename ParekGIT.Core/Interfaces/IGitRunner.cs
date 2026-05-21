using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
    public interface IGitRunner
    {
        Task<string> ExecuteCommandAsync(string repositoryPath, string arguments);

        // Parsers
        Task<IEnumerable<GitBranch>> GetBranchesAsync(string repositoryPath);
        Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repositoryPath);

        // Commands
        Task CheckoutBranchAsync(string repositoryPath, string branchName, bool isRemote);
        Task CreateBranchAsync(string repositoryPath, string branchName);
        Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files);

        Task<GitRepository> CreateRepositoryAsync(string repoName, string localPath, string gitIgnore, string license);
    }
}
