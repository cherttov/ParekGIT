using ParekGIT.Core.Data;
using ParekGIT.UI.Data;
using ParekGIT.UI.Handlers;
using ParekGIT.UI.Ipc;
using Photino.NET;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        // Individual handlers
        private static async Task HandleAppReady(PhotinoWindow window, LiteDbStore dbStore)
        {
            var repos = await dbStore.GetAllRepositoriesAsync();

            var response = new IpcMessage
            {
                Action = "LOAD_REPOSITORIES",
                Payload = JsonSerializer.SerializeToElement(repos)
            };

            string jsonString = JsonSerializer.Serialize(response);

            window.SendWebMessage(jsonString);
        }

        private static async Task HandleRepoSelected(PhotinoWindow window, LiteDbStore dbStore, string absolutePath)
        {
            Console.WriteLine($"Selected repo: {absolutePath}");
        }
    }
}
