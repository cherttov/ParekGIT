using System.Text.Json;

namespace ParekGIT.Bridge.Interfaces
{
    public interface IMessageHandler
    {
        string Action { get; }
        Task ExecuteAsync(JsonElement payload);
    }
}
