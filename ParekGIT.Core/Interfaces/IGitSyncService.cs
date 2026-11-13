namespace ParekGIT.Core.Interfaces
{
	public interface IGitSyncService
	{
		Task PushAsync(string repoPath, CancellationToken cancellationToken = default);
		Task PullAsync(string repoPath, CancellationToken cancellationToken = default);
		Task FetchRepositoryAsync(string repoPath, CancellationToken cancellationToken = default);
	}
}
