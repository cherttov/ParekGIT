using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
    public interface IGitRunner
    {
        Task<string> ExecuteCommandAsync(string repositoryPath, string arguments);

        // Parsers
        Task<IEnumerable<GitBranch>> GetBranchesAsync(string repositoryPath);

        // Commands
        Task CheckoutBranchAsync(string repositoryPath, string branchName, bool isRemote);
        Task CreateBranchAsync(string repositoryPath, string branchName);
    }
}
