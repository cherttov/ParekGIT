using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class CommitCheckoutHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitHistoryService _historyService;

		public string Action => "COMMIT_CHECKOUT";

		// Constructor
		public CommitCheckoutHandler(PhotinoWindow window, IGitHistoryService historyService)
		{
			_window = window;
			_historyService = historyService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string commitHash = payload.GetProperty("commitHash").GetString()
							  ?? throw new IpcPayloadException("commitHash");

			await _historyService.CheckoutCommitAsync(repoPath, commitHash);

			// Response
			var response = IpcMessage.Create("COMMIT_CHECKED_OUT", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
