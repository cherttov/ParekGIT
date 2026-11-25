using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class ChangeIgnoreHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitWorkingTreeService _workingTreeService;

		public string Action => "CHANGE_IGNORE";

		// Constructor
		public ChangeIgnoreHandler(PhotinoWindow window, IGitWorkingTreeService workingTreeService)
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

			string ignoreType = payload.GetProperty("ignoreType").GetString()
				?? throw new IpcPayloadException("ignoreType");

			switch (ignoreType)
			{
				case "file":
					await _workingTreeService.IgnoreFileAsync(repoPath, filePath);
					break;

				case "folder":
					await _workingTreeService.IgnoreFolderAsync(repoPath, filePath);
					break;

				case "extension":
					await _workingTreeService.IgnoreExtensionAsync(repoPath, filePath);
					break;
				default:
					throw new IpcPayloadException("ignoreType", $"unrecognized value '{ignoreType}'");
			}

			// Response
			var response = IpcMessage.Create("CHANGE_IGNORED", new { repoPath });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
