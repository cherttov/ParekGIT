using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Services;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;

namespace ParekGIT.Core
{
	public static class CoreServiceExtensions
	{
		public static IServiceCollection AddCoreServices(this IServiceCollection services)
		{
			services.AddSingleton<GitCommandExecutor>();
			services.AddSingleton<IAppDataPaths, AppDataPaths>();
			services.AddSingleton<ILogger, FileLogger>();
			services.AddSingleton<IFileSystemService, FileSystemService>();
			services.AddSingleton<IRepositoryWatcher, RepositoryWatcher>();
			services.AddSingleton<ITemplateService, TemplateService>();

			services.AddGitServices();

			return services;
		}

		private static IServiceCollection AddGitServices(this IServiceCollection services)
		{
			services.AddSingleton<IGitStatusService, GitStatusService>();
			services.AddSingleton<IGitBranchService, GitBranchService>();
			services.AddSingleton<IGitHistoryService, GitHistoryService>();
			services.AddSingleton<IGitWorkingTreeService, GitWorkingTreeService>();
			services.AddSingleton<IGitSyncService, GitSyncService>();
			services.AddSingleton<IGitConfigService, GitConfigService>();
			services.AddSingleton<IGitStashService, GitStashService>();
			services.AddSingleton<IGitRepositoryService, GitRepositoryService>();

			return services;
		}
	}
}
