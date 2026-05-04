using ParekGIT.Core.Data;
using ParekGIT.UI.Data;
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
                .Load("wwwroot/index.html")
                .RegisterWebMessageReceivedHandler(async (object sender, string message) =>
                {
                    var window = (PhotinoWindow)sender;

                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var ipcMessage = JsonSerializer.Deserialize<IpcMessage>(message, options);

                        if (ipcMessage == null) return;

                        switch (ipcMessage.Action)
                        {
                            case "APP_READY":
                                await HandleAppReady(window, dbStore);
                                break;

                            default:
                                Console.WriteLine($"Unknown action: {ipcMessage.Action}");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to parse IPC Message: {ex.Message}");
                    }
                });

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
    }
}
