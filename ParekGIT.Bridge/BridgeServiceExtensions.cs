using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Bridge.Handlers;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Ipc;
using ParekGIT.Bridge.Services;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;

namespace ParekGIT.Data
{
	public static class BridgeServiceExtensions
	{
		public static IServiceCollection AddBridgeServices(this IServiceCollection services)
		{
			services.AddSingleton<IRepositoryWatcher, RepositoryWatcher>();
			services.AddSingleton<IRemoteSyncNotifier, RemoteSyncNotifier>();
			services.AddSingleton<IpcRouter>();

			services.AddIpcHandlers();

			return services;
		}

		private static IServiceCollection AddIpcHandlers(this IServiceCollection services)
		{
			// App
			services.AddSingleton<IMessageHandler, AppReadyHandler>();

			// Repo
			services.AddSingleton<IMessageHandler, RepoOpenHandler>();
			services.AddSingleton<IMessageHandler, RepoCloneHandler>();
			services.AddSingleton<IMessageHandler, RepoAddHandler>();
			services.AddSingleton<IMessageHandler, RepoCreateHandler>();
			services.AddSingleton<IMessageHandler, RepoRemoveHandler>();
			services.AddSingleton<IMessageHandler, RepoPullHandler>();
			services.AddSingleton<IMessageHandler, RepoPushHandler>();
			services.AddSingleton<IMessageHandler, RepoStatusLoadHandler>();
			services.AddSingleton<IMessageHandler, RepoTerminalHandler>();
			services.AddSingleton<IMessageHandler, RepoFetchHandler>();
			services.AddSingleton<IMessageHandler, RepoWatcherHandler>();

			// Branch
			services.AddSingleton<IMessageHandler, BranchCheckoutHandler>();
			services.AddSingleton<IMessageHandler, BranchListLoadHandler>();
			services.AddSingleton<IMessageHandler, BranchCreateHandler>();
			services.AddSingleton<IMessageHandler, BranchCreateFromCommitHandler>();
			services.AddSingleton<IMessageHandler, BranchRenameHandler>();
			services.AddSingleton<IMessageHandler, BranchDeleteHandler>();
			services.AddSingleton<IMessageHandler, BranchForceDeleteHandler>();
			services.AddSingleton<IMessageHandler, BranchHistoryLoadHandler>();
			services.AddSingleton<IMessageHandler, BranchMergeHandler>();
			services.AddSingleton<IMessageHandler, BranchPublishHandler>();

			// Commit
			services.AddSingleton<IMessageHandler, CommitFileDiffLoadHandler>();
			services.AddSingleton<IMessageHandler, CommitCheckoutHandler>();
			services.AddSingleton<IMessageHandler, CommitRevertHandler>();
			services.AddSingleton<IMessageHandler, CommitDetailsLoadHandler>();

			// Change
			services.AddSingleton<IMessageHandler, ChangeDiffLoadHandler>();
			services.AddSingleton<IMessageHandler, ChangeAllDiscardHandler>();
			services.AddSingleton<IMessageHandler, ChangeDiscardHandler>();
			services.AddSingleton<IMessageHandler, ChangeIgnoreHandler>();
			services.AddSingleton<IMessageHandler, ChangesCommitHandler>();

			// Explorer
			services.AddSingleton<IMessageHandler, ExplorerPickFolderHandler>();
			services.AddSingleton<IMessageHandler, ExplorerShowHandler>();

			// Settings / Themes
			services.AddSingleton<IMessageHandler, SettingsSaveHandler>();
			services.AddSingleton<IMessageHandler, ThemeListLoadHandler>();

			// Todo
			services.AddSingleton<IMessageHandler, TodoLoadHandler>();
			services.AddSingleton<IMessageHandler, TodoSaveHandler>();

			// Config
			services.AddSingleton<IMessageHandler, ConfigLocalLoadHandler>();
			services.AddSingleton<IMessageHandler, ConfigLocalSaveHandler>();
			services.AddSingleton<IMessageHandler, ConfigGlobalLoadHandler>();
			services.AddSingleton<IMessageHandler, ConfigGlobalSaveHandler>();

			// Stash
			services.AddSingleton<IMessageHandler, StashListLoadHandler>();
			services.AddSingleton<IMessageHandler, StashSaveHandler>();
			services.AddSingleton<IMessageHandler, StashPopHandler>();
			services.AddSingleton<IMessageHandler, StashDropHandler>();

			// Logs
			services.AddSingleton<IMessageHandler, LogsClearHandler>();

			return services;
		}
	}
}
