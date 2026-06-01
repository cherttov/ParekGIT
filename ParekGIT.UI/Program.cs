using ParekGIT.Data.Data;
using Photino.NET;
using System.Drawing;
using ParekGIT.Bridge.Ipc;
using ParekGIT.Bridge.Handlers;
using ParekGIT.Core.Git;
using ParekGIT.Core.Services;

namespace ParekGIT.UI
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Database
            var dbStore = new LiteDbStore();

            // Core runner
            var gitRunner = new GitCliRunner();

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
                .Load("wwwroot/index.html");

            // Setup IPC router
            var router = new IpcRouter();

            router.RegisterHandler(new AppReadyHandler(window, dbStore));

            router.RegisterHandler(new RepoSelectedHandler(window, dbStore, gitRunner, repoWatcher));
            router.RegisterHandler(new RepoAddHandler(window, dbStore, gitRunner));
            router.RegisterHandler(new RepoCreateHandler(window, dbStore, gitRunner));
            router.RegisterHandler(new RepoRemoveHandler(window, dbStore, gitRunner));
            router.RegisterHandler(new RepoStatusHandler(window, gitRunner));
            router.RegisterHandler(new RepoExplorerHandler());
            router.RegisterHandler(new RepoTerminalHandler());
            router.RegisterHandler(new RepoCommitHandler(window, gitRunner));
            router.RegisterHandler(new RepoFetchHandler(window, gitRunner));
            router.RegisterHandler(new RepoWatcherHandler(window, repoWatcher));

            router.RegisterHandler(new BranchSelectedHandler(window, dbStore, gitRunner));
            router.RegisterHandler(new BranchCreateHandler(window, gitRunner));
            router.RegisterHandler(new BranchRenameHandler(window, gitRunner));
            router.RegisterHandler(new BranchDeleteHandler(window, gitRunner));
            router.RegisterHandler(new BranchHistoryHandler(window, gitRunner));

            router.RegisterHandler(new FileDiffHandler(window, gitRunner));

            router.RegisterHandler(new ExplorerDialogHandler(window));            

            window.RegisterWebMessageReceivedHandler(router.HandleMessage);

            window.WaitForClose();
        }
    }
}
