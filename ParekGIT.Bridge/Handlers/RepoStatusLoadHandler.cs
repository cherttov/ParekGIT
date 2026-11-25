using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoStatusLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitStatusService _statusService;
		private readonly IFileSystemService _fileSystem;

		public string Action => "REPO_STATUS_LOAD";

		// Constructor
		public RepoStatusLoadHandler(PhotinoWindow window, IGitStatusService statusService, IFileSystemService fileSystem)
		{
			_window = window;
			_statusService = statusService;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			// Invalid repoPath -> empty response
			if (string.IsNullOrEmpty(repoPath) || !_fileSystem.DirectoryExists(repoPath))
			{
				var emptyResponse = IpcMessage.Create("REPO_STATUS_LOADED", new { files = Array.Empty<GitFileStatus>() });
				_window.SendWebMessage(JsonSerializer.Serialize(emptyResponse));
				return;
			}

			IEnumerable<GitFileStatus> files = await _statusService.GetStatusAsync(repoPath);

			// Response
			var response = IpcMessage.Create("REPO_STATUS_LOADED", new { files });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
