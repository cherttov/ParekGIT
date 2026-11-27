using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Git.Services
{
	internal static class GitStatusServiceExtensions
	{
		internal static async Task EnsureCleanWorkingTreeAsync(this IGitStatusService status, string repoPath)
		{
			if ((await status.GetStatusAsync(repoPath)).Any())
				throw new Exception($"Uncommitted changes are present. Please commit, stash or discard them first.");
		}
	}
}
