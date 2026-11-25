using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class ChangeDiscardHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitWorkingTreeService _workingTreeService;

		public string Action => "CHANGE_DISCARD";

		// Constructor
		public ChangeDiscardHandler(PhotinoWindow window, IGitWorkingTreeService workingTreeService)
		{
			_window = window;
			_workingTreeService = workingTreeService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string filePath = payload.GetProperty("filePath").GetString()
				?? throw new IpcPayloadException("filePath");

			await _workingTreeService.DiscardChangeAsync(repoPath, filePath);

			// Response
			var response = IpcMessage.Create("CHANGE_DISCARDED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
