using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
    internal interface IRepositoryStore
    {
        Task<IEnumerable<GitRepository>> GetallRepositoriesAsync();
        Task UpsertRepositoryAsync(GitRepository repository); // Update || Insert
        Task DeleteRepositoryAsync(Guid id);
    }
}
