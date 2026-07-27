using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ExplorerDialogHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;

		public string Action => "EXPLORER_OPEN_DIALOG";

		// Constructor
		public ExplorerDialogHandler(PhotinoWindow window)
		{
			_window = window;
		}

		public Task ExecuteAsync(JsonElement payload)
		{
			string[] selectedDir = _window.ShowOpenFolder(multiSelect: false);

			if (selectedDir != null && selectedDir.Length > 0)
			{
				// Response (success)
				var response = new IpcMessage
				{
					Action = "FOLDER_SELECTED",
					Payload = JsonSerializer.SerializeToElement(new { path = selectedDir[0] })
				};

				_window.SendWebMessage(JsonSerializer.Serialize(response));
			}
			else
			{
				// Response (cancel)
				var response = new IpcMessage
				{
					Action = "FOLDER_CANCELED",
					Payload = JsonSerializer.SerializeToElement(new { })
				};

				_window.SendWebMessage(JsonSerializer.Serialize(response));
			}
			return Task.CompletedTask;
		}
	}
}
