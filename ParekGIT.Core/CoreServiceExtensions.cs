using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Services;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core
{
	public static class CoreServiceExtensions
	{
		public static IServiceCollection AddGitServices(this IServiceCollection services)
		{
			services.AddSingleton<GitCommandExecutor>();
			services.AddSingleton<IGitStatusService, GitStatusService>();
			services.AddSingleton<IGitBranchService, GitBranchService>();
			services.AddSingleton<IGitHistoryService, GitHistoryService>();
			services.AddSingleton<IGitWorkingTreeService, GitWorkingTreeService>();
			services.AddSingleton<IGitSyncService, GitSyncService>();
			services.AddSingleton<IGitConfigService, GitConfigService>();
			services.AddSingleton<IGitStashService, GitStashService>();
			services.AddSingleton<IGitRepoService, GitRepoService>();
			return services;
		}
	}
}
