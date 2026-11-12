using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitStashService
	{
		Task<IEnumerable<GitStash>> GetStashesAsync(string repoPath);
		Task StashSaveAsync(string repoPath, string message, IEnumerable<string> files);
		Task StashPopAsync(string repoPath, string stashSelector);
		Task StashDropAsync(string repoPath, string stashSelector);
	}
}
