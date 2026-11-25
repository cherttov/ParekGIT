using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class ChangeDiffLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitStatusService _statusService;

		public string Action => "CHANGE_DIFF_LOAD";

		// Constructor
		public ChangeDiffLoadHandler(PhotinoWindow window, IGitStatusService statusService)
		{
			_window = window;
			_statusService = statusService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string filePath = payload.GetProperty("filePath").GetString()
				?? throw new IpcPayloadException("filePath");

			string diffText = await _statusService.GetFileDiffAsync(repoPath, filePath);

			// Response
			var response = IpcMessage.Create("CHANGE_DIFF_LOADED", new { diffText });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
