using System.Text.Json;

namespace ParekGIT.Bridge.Data
{
	internal class IpcMessage
	{
		public string Action { get; set; } = string.Empty;
		public JsonElement Payload { get; set; }
	}
}
