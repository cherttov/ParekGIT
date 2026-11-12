namespace ParekGIT.Core.Interfaces
{
	public interface IGitSyncService
	{
		Task PushAsync(string repoPath);
		Task PullAsync(string repoPath);
		Task FetchRepositoryAsync(string repoPath);
	}
}
