using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitStatusService
	{
		Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repoPath);
		Task<string> GetFileDiffAsync(string repoPath, string filePath);
		Task<string> GetHistoryFileDiffAsync(string repoPath, string commitHash, string filePath);
		Task<int> GetCommitsAheadAsync(string repoPath);
		Task<int> GetCommitsBehindAsync(string repoPath);
		Task<string?> GetRemoteUrlAsync(string repoPath);
	}
}
