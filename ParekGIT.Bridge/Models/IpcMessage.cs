using System.Text.Json;

namespace ParekGIT.Bridge.Data
{
	internal class IpcMessage
	{
		public string Action { get; set; } = string.Empty;
		public JsonElement Payload { get; set; }

		public static readonly JsonSerializerOptions JsonOptions = new()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		public static IpcMessage Create<T>(string action, T payload)
		{
			return new IpcMessage()
			{
				Action = action, 
				Payload = JsonSerializer.SerializeToElement(payload, JsonOptions)
			};
		}
	}
}
