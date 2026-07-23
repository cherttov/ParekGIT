using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class HistoryCheckoutHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "HISTORY_CHECKOUT";

		// Constructor
		public HistoryCheckoutHandler(PhotinoWindow window, IGitRunner gitRunner)
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

			await _gitRunner.CheckoutCommitAsync(repoPath, commitHash);

			// Response
			var response = new IpcMessage
			{
				Action = "HISTORY_CHECKED_OUT",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
