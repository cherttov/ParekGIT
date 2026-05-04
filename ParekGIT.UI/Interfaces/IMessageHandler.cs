using System.Text.Json;

namespace ParekGIT.UI.Interfaces
{
    internal interface IMessageHandler
    {
        string Action { get; }
        Task ExecuteAsync(JsonElement payload);
    }
}
