using ParekGIT.Core.Models;

namespace ParekGIT.Data.Interfaces
{
    public interface IRepositoryStore
    {
        Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync();
        Task<GitRepository?> GetRepositoryByPathAsync(string absolutePath);
        Task UpsertRepositoryAsync(GitRepository repository); // Update || Insert
        Task DeleteRepositoryAsync(Guid id);
        Task UpdateRepositoryValidStateAsync(string absolutePath, bool isValid);
    }
}
