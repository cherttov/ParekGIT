using ParekGIT.Core.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface IRepositoryStore
	{
		Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync();
		Task<GitRepository?> GetRepositoryAsync(string absolutePath);
		Task UpsertRepositoryAsync(GitRepository repository); // Update || Insert
		Task DeleteRepositoryAsync(string absolutePath);
		Task UpdateRepositoryValidStateAsync(string absolutePath, bool isValid);
	}
}
