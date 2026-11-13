using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchHistoryLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitHistoryService _historyService;

		public string Action => "BRANCH_HISTORY_LOAD";

		// Constructor
		public BranchHistoryLoadHandler(PhotinoWindow window, IGitHistoryService historyService)
		{
			_window = window;
			_historyService = historyService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string branchName = payload.GetProperty("branchName").GetString()
								?? throw new IpcPayloadException("branchName");

			int skip = payload.TryGetProperty("skip", out var skipProperty) && skipProperty.TryGetInt32(out int parsedSkip)
				? parsedSkip : 0;

			int take = payload.TryGetProperty("take", out var takeProperty) && skipProperty.TryGetInt32(out int parsedTake)
				? parsedTake : 50;

			IEnumerable<GitCommit> commits = await _historyService.GetBranchHistoryAsync(repoPath, branchName, skip, take);

			// Response
			var response = IpcMessage.Create(skip > 0 ? "BRANCH_HISTORY_APPENDED" : "BRANCH_HISTORY_LOADED", new { commits });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
