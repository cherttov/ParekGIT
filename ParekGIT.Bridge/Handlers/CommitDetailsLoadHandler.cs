using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class CommitDetailsLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitHistoryService _historyService;

		public string Action => "COMMIT_DETAILS_LOAD";

		// Constructor
		public CommitDetailsLoadHandler(PhotinoWindow window, IGitHistoryService historyService)
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

			CommitDetailsResult details = await _historyService.GetCommitDetailsAsync(repoPath, commitHash);

			// Response
			var response = IpcMessage.Create("COMMIT_DETAILS_LOADED", new
			{
				author = details.Author,
				message = details.Message,
				files = details.Files
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
