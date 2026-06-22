using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Ipc
{
    public class IpcRouter
    {
        private readonly Dictionary<string, IMessageHandler> _handlers = new();

        public void RegisterHandler(IMessageHandler handler)
        {
            _handlers[handler.Action] = handler;
        }

        public async void HandleMessage(object sender, string message)
        {
            var window = (PhotinoWindow)sender;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var ipcMessage = JsonSerializer.Deserialize<IpcMessage>(message, options);

                if (ipcMessage == null) { return; }

                if (_handlers.TryGetValue(ipcMessage.Action, out var handler))
                {
                    await handler.ExecuteAsync(ipcMessage.Payload);
                }
                else
                {
                    string warning = $"No handler found for action: {ipcMessage.Action}";
                    Console.WriteLine(warning);

                    var unknownActionError = new IpcMessage
                    {
                        Action = "APP_ERROR",
                        Payload = JsonSerializer.SerializeToElement(new { message = warning })
                    };
                    window.SendWebMessage(JsonSerializer.Serialize(unknownActionError));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to process IPC Message: {ex.Message}");

                var error = new IpcMessage
                {
                    Action = "APP_ERROR",
                    Payload = JsonSerializer.SerializeToElement(new { message = ex.Message })
                };
                window.SendWebMessage(JsonSerializer.Serialize(error));
            }
        }
    }
}
