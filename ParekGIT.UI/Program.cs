using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Drawing;
using ParekGIT.Bridge.Ipc;
using ParekGIT.Bridge.Handlers;
using ParekGIT.Core.Git;
using ParekGIT.Core.Services;
using ParekGIT.Bridge.Services;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.UI
{
	internal class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				var services = new ServiceCollection();

				// ======================== Core Services ========================
				services.AddSingleton<ILogger, FileLogger>();
				services.AddSingleton<IFileSystemService, FileSystemService>();

				services.AddSingleton<LiteDbStore>();
				services.AddSingleton<JsonStore>();
				services.AddSingleton<IRepositoryStore>(p => p.GetRequiredService<LiteDbStore>());
				services.AddSingleton<ITodoStore>(p => p.GetRequiredService<LiteDbStore>());
				services.AddSingleton<ISettingsStore>(p => p.GetRequiredService<JsonStore>());
				services.AddSingleton<IThemeStore>(p => p.GetRequiredService<JsonStore>());

				services.AddSingleton<ITemplateService, TemplateService>();
				services.AddSingleton<IGitRunner, GitCliRunner>();
				services.AddSingleton<IRepoWatcher, RepoWatcher>();
				services.AddSingleton<IRemoteSyncNotifier, RemoteSyncNotifier>();
				services.AddSingleton<IpcRouter>();

				// ======================== Window ========================
				services.AddSingleton(provider =>
				{
					return new PhotinoWindow()
						.SetTitle("ParekGIT")
						.SetUseOsDefaultSize(false)
						.SetSize(new Size(960, 660))
						.SetMinSize(960, 660)
						.Center()
						.SetContextMenuEnabled(true) // later false
						.SetDevToolsEnabled(true) // later false
						.SetIconFile("wwwroot/assets/icons/icon.ico")
						.SetJavascriptClipboardAccessEnabled(true)
						.Load("wwwroot/index.html");
				});

				// ======================== Ipc handlers ========================
				// App
				services.AddSingleton<IMessageHandler, AppReadyHandler>();

				// Repo
				services.AddSingleton<IMessageHandler, RepoSelectedHandler>();
				services.AddSingleton<IMessageHandler, RepoCloneHandler>();
				services.AddSingleton<IMessageHandler, RepoAddHandler>();
				services.AddSingleton<IMessageHandler, RepoCreateHandler>();
				services.AddSingleton<IMessageHandler, RepoRemoveHandler>();
				services.AddSingleton<IMessageHandler, RepoPullHandler>();
				services.AddSingleton<IMessageHandler, RepoPushHandler>();
				services.AddSingleton<IMessageHandler, RepoPublishHandler>();
				services.AddSingleton<IMessageHandler, RepoStatusLoadHandler>();
				services.AddSingleton<IMessageHandler, RepoTerminalHandler>();
				services.AddSingleton<IMessageHandler, RepoCommitHandler>();
				services.AddSingleton<IMessageHandler, RepoFetchHandler>();
				services.AddSingleton<IMessageHandler, RepoWatcherHandler>();

				// Branch
				services.AddSingleton<IMessageHandler, BranchSelectedHandler>();
				services.AddSingleton<IMessageHandler, BranchListLoadHandler>();
				services.AddSingleton<IMessageHandler, BranchCreateHandler>();
				services.AddSingleton<IMessageHandler, BranchCreateFromCommitHandler>();
				services.AddSingleton<IMessageHandler, BranchRenameHandler>();
				services.AddSingleton<IMessageHandler, BranchDeleteHandler>();
				services.AddSingleton<IMessageHandler, BranchForceDeleteHandler>();
				services.AddSingleton<IMessageHandler, BranchHistoryLoadHandler>();
				services.AddSingleton<IMessageHandler, BranchMergeHandler>();

				// Commit/File/History
				services.AddSingleton<IMessageHandler, FileDiffLoadHandler>();
				services.AddSingleton<IMessageHandler, CommitFileDiffLoadHandler>();
				services.AddSingleton<IMessageHandler, CommitCheckoutHandler>();
				services.AddSingleton<IMessageHandler, CommitRevertHandler>();
				services.AddSingleton<IMessageHandler, CommitDetailsLoadHandler>();

				// Change
				services.AddSingleton<IMessageHandler, ChangeDiscardHandler>();
				services.AddSingleton<IMessageHandler, ChangeIgnoreHandler>();

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

				// ======================== Building App, DependencyInjection & Handlers ========================
				using var serviceProvider = services.BuildServiceProvider();

				// Initialize app data directories and files
				var fileSystem = serviceProvider.GetRequiredService<IFileSystemService>();
				foreach (string dir in AppDataPaths.GetAllDirectories())
				{
					fileSystem.CreateDirectory(dir).GetAwaiter().GetResult();
				}
				var settingsStore = serviceProvider.GetRequiredService<ISettingsStore>();
				settingsStore.EnsureSettingsFileAsync().GetAwaiter().GetResult();
				var themeStore = serviceProvider.GetRequiredService<IThemeStore>();
				themeStore.EnsureDefaultThemesAsync().GetAwaiter().GetResult();

				var window = serviceProvider.GetRequiredService<PhotinoWindow>();
				var router = serviceProvider.GetRequiredService<IpcRouter>();
				_ = serviceProvider.GetRequiredService<IRemoteSyncNotifier>();

				var allHandlers = serviceProvider.GetServices<IMessageHandler>();
				foreach (var handler in allHandlers)
				{
					router.RegisterHandler(handler);
				}

				// ======================== Start the application ========================
				window.RegisterWebMessageReceivedHandler(router.HandleMessage!);

				var app = new PhotinoApplication();
				app.Run(window);
			}
			catch (Exception ex)
			{
				string errorMessage = "Error occurred while starting ParekGIT";

				if (OperatingSystem.IsLinux())
				{
					Console.WriteLine("Error occured while starting ParekGIT:");
					Console.WriteLine("- WebKitGTK might be missing.");
					Console.WriteLine($"- Message: {ex.Message}");
				}
				else
				{
					Console.WriteLine("Error occured while starting ParekGIT:");
					Console.WriteLine($"- Message: {ex.Message}");
				}

				// Try logging startup error
				try
				{
					var logger = new FileLogger();
					logger.LogErrorAsync(errorMessage, ex).GetAwaiter().GetResult();
				}
				catch { }
			}
		}
	}
}
