using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class FileDiffHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "GET_FILE_DIFF";

		// Constructor
		public FileDiffHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			string filePath = payload.GetProperty("filePath").GetString()
				?? throw new IpcPayloadException("filePath");

			string diffText = await _gitRunner.GetFileDiffAsync(repoPath, filePath);

			// Response
			var response = new IpcMessage
			{
				Action = "FILE_DIFF_LOADED",
				Payload = JsonSerializer.SerializeToElement(new { diffText = diffText })
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
