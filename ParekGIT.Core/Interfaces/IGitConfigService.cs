using ParekGIT.Core.Models;

namespace ParekGIT.Core.Interfaces
{
	public interface IGitConfigService
	{
		Task<GitConfigInfo> GetGlobalConfigAsync(string? repoPath = null);
		Task<GitConfigInfo> GetLocalConfigAsync(string repoPath);
		Task SaveLocalConfigAsync(string repoPath, string name, string email);
		Task SaveGlobalConfigAsync(string? repoPath, string name, string email);
	}
}
