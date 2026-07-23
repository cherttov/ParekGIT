using ParekGIT.Data.Data;
using Photino.NET;
using System.Drawing;
using ParekGIT.Bridge.Ipc;
using ParekGIT.Bridge.Handlers;
using ParekGIT.Core.Git;
using ParekGIT.Core.Services;
using System.Runtime.InteropServices;

namespace ParekGIT.UI
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // Logger
                var logger = new FileLogger();

                // Database
                var dbStore = new LiteDbStore();

                // OS Services
                var fileSystem = new FileSystemService(logger);

                // Template service (creating .gitignore & LICENSE files)
                var templateService = new TemplateService(fileSystem, logger);

                // Core runner
                var gitRunner = new GitCliRunner(fileSystem, templateService, logger);

                // Repo watcher
                var repoWatcher = new RepoWatcher();

                // Window
                var window = new PhotinoWindow()
                    .SetTitle("ParekGIT")
                    .SetUseOsDefaultSize(false)
                    .SetSize(new Size(960, 660))
                    .SetMinSize(960, 660)
                    .Center()
                    .SetContextMenuEnabled(true) // later false
                    .SetDevToolsEnabled(true) // later false
                    .SetJavascriptClipboardAccessEnabled(true)
                    .Load("wwwroot/index.html");

                // Setup IPC router
                var router = new IpcRouter(logger);

                router.RegisterHandler(new AppReadyHandler(window, dbStore, fileSystem));

                router.RegisterHandler(new RepoSelectedHandler(window, dbStore, gitRunner, repoWatcher, fileSystem));
                router.RegisterHandler(new RepoCloneHandler(window, dbStore, gitRunner));
                router.RegisterHandler(new RepoAddHandler(window, dbStore, fileSystem));
                router.RegisterHandler(new RepoCreateHandler(window, dbStore, gitRunner));
                router.RegisterHandler(new RepoRemoveHandler(window, dbStore, fileSystem, logger));
                router.RegisterHandler(new RepoPullHandler(window, gitRunner));
                router.RegisterHandler(new RepoStatusHandler(window, gitRunner, fileSystem));
                router.RegisterHandler(new RepoTerminalHandler(fileSystem));
                router.RegisterHandler(new RepoCommitHandler(window, dbStore, gitRunner));
                router.RegisterHandler(new RepoFetchHandler(window, gitRunner));
                router.RegisterHandler(new RepoWatcherHandler(window, repoWatcher));

                router.RegisterHandler(new BranchSelectedHandler(window, gitRunner));
                router.RegisterHandler(new BranchCreateHandler(window, gitRunner));
                router.RegisterHandler(new BranchHistoryCreateHandler(window, gitRunner));
                router.RegisterHandler(new BranchRenameHandler(window, gitRunner));
                router.RegisterHandler(new BranchDeleteHandler(window, gitRunner));
                router.RegisterHandler(new BranchHistoryHandler(window, gitRunner));
                router.RegisterHandler(new BranchMergeHandler(window, gitRunner));

                router.RegisterHandler(new FileDiffHandler(window, gitRunner));
                router.RegisterHandler(new HistoryFileDiffHandler(window, gitRunner));
                router.RegisterHandler(new CommitDetailsHandler(window, gitRunner));

                router.RegisterHandler(new ChangeDiscardHandler(window, gitRunner));
                router.RegisterHandler(new ChangeIgnoreHandler(window, gitRunner));

                router.RegisterHandler(new ExplorerDialogHandler(window));
                router.RegisterHandler(new ExplorerOpenHandler(fileSystem));

                router.RegisterHandler(new SettingsSaveHandler(window, dbStore));

                router.RegisterHandler(new TodoLoadHandler(window, dbStore));
                router.RegisterHandler(new TodoSaveHandler(window, dbStore));

                router.RegisterHandler(new ConfigLocalGetHandler(window, gitRunner));
                router.RegisterHandler(new ConfigLocalSaveHandler(window, gitRunner));
                router.RegisterHandler(new ConfigGlobalGetHandler(window, gitRunner));
                router.RegisterHandler(new ConfigGlobalSaveHandler(window, gitRunner));

                router.RegisterHandler(new LogsClearHandler(window, fileSystem, logger));

                window.RegisterWebMessageReceivedHandler(router.HandleMessage!);

                window.WaitForClose();
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occurred while starting ParekGIT";

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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
