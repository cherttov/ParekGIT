using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitRepoService
	{
		Task<GitRepository> CreateRepositoryAsync(CreateRepoRequest request);
		Task CloneRepositoryAsync(string repoUrl, bool asLocal, string localPath, CancellationToken cancellationToken = default);
	}
}
