using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class HistoryRevertHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "HISTORY_REVERT";

		// Constructor
		public HistoryRevertHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string commitHash = payload.GetProperty("commitHash").GetString()
							  ?? throw new IpcPayloadException("commitHash");

			await _gitRunner.RevertCommitAsync(repoPath, commitHash);

			// Response
			var response = new IpcMessage
			{
				Action = "HISTORY_REVERT_RESULT",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
