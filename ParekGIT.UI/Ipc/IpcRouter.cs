using ParekGIT.UI.Data;
using ParekGIT.UI.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.UI.Ipc
{
    internal class IpcRouter
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

                if (ipcMessage == null) return;

                if (_handlers.TryGetValue(ipcMessage.Action, out var handler))
                {
                    await handler.ExecuteAsync(ipcMessage.Payload);
                }
                else
                {
                    Console.WriteLine($"No handler found for action: {ipcMessage.Action}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to process IPC Message: {ex.Message}");
            }
        }
    }
}
