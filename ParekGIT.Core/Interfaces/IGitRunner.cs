using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
    public interface IGitRunner
    {
        Task<string> ExecuteCommandAsync(string repositoryPath, string arguments);
        Task<IEnumerable<GitBranch>> GetBranchesAsync(string repositoryPath);
        Task CheckoutBranchAsync(string repositoryPath, string branchName, bool isRemote);
    }
}
