using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class ChangeIgnoreHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "CHANGE_IGNORE";

		// Constructor
		public ChangeIgnoreHandler(PhotinoWindow window, IGitRunner gitRunner)
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

			string ignoreType = payload.GetProperty("ignoreType").GetString()
				?? throw new IpcPayloadException("ignoreType");

			switch (ignoreType)
			{
				case "file":
					await _gitRunner.IgnoreFileAsync(repoPath, filePath);
					break;

				case "folder":
					await _gitRunner.IgnoreFolderAsync(repoPath, filePath);
					break;

				case "extension":
					await _gitRunner.IgnoreExtensionAsync(repoPath, filePath);
					break;
				default:
					throw new IpcPayloadException("ignoreType", $"unrecognized value '{ignoreType}'");
			}

			// Response
			var response = new IpcMessage
			{
				Action = "CHANGE_IGNORED",
				Payload = JsonSerializer.SerializeToElement(new { repoPath = repoPath })
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
