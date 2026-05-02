using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
    public interface IRepositoryStore
    {
        Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync();
        Task UpsertRepositoryAsync(GitRepository repository); // Update || Insert
        Task DeleteRepositoryAsync(Guid id);
    }
}
