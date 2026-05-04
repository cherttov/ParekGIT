using System.Text.Json;

namespace ParekGIT.UI.Data
{
    internal class IpcMessage
    {
        public string Action { get; set; }
        public JsonElement Payload { get; set; }
    }
}
