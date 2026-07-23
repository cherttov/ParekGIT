using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoStatusHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;
		private readonly IFileSystemService _fileSystem;

		public string Action => "GET_REPO_STATUS";

		// Constructor
		public RepoStatusHandler(PhotinoWindow window, IGitRunner gitRunner, IFileSystemService fileSystem)
		{
			_window = window;
			_gitRunner = gitRunner;
			_fileSystem = fileSystem;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			// Invalid repoPath -> empty response
			if (string.IsNullOrEmpty(repoPath) || !_fileSystem.DirectoryExists(repoPath))
			{
				var emptyResponse = new IpcMessage
				{
					Action = "REPO_STATUS_LOADED",
					Payload = JsonSerializer.SerializeToElement(new List<object>())
				};
				_window.SendWebMessage(JsonSerializer.Serialize(emptyResponse));
				return;
			}

			IEnumerable<GitFileStatus> changedFiles = await _gitRunner.GetStatusAsync(repoPath);

			// Response
			var response = new IpcMessage
			{
				Action = "REPO_STATUS_LOADED",
				Payload = JsonSerializer.SerializeToElement(changedFiles)
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
