using ParekGIT.Data.Data;
using Photino.NET;
using System.Drawing;
using ParekGIT.Bridge.Ipc;
using ParekGIT.Bridge.Handlers;

namespace ParekGIT.UI
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Database
            var dbStore = new LiteDbStore();

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
            router.RegisterHandler(new RepoSelectedHandler(window, dbStore));

            window.RegisterWebMessageReceivedHandler(router.HandleMessage);

            window.WaitForClose();
        }
    }
}
