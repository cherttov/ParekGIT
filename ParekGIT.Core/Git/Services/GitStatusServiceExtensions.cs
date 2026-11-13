using ParekGIT.Core.Interfaces;
using System.Runtime.CompilerServices;

namespace ParekGIT.Core.Git.Services
{
	internal static class GitStatusServiceExtensions
	{
		internal static async Task EnsureCleanWorkingTreeAsync(this IGitStatusService status, string repoPath, [CallerMemberName] string caller = "")
		{
			if ((await status.GetStatusAsync(repoPath)).Any())
				throw new Exception($"Uncommitted changes are present [{caller}]");
		}
	}
}
