using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchHistoryHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "GET_BRANCH_HISTORY";

		// Constructor
		public BranchHistoryHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string branchName = payload.GetProperty("branchName").GetString()
								?? throw new IpcPayloadException("branchName");

			int skip = payload.TryGetProperty("skip", out var skipProperty) && skipProperty.TryGetInt32(out int parsedSkip)
				? parsedSkip : 0;

			int take = payload.TryGetProperty("skip", out var takeProperty) && skipProperty.TryGetInt32(out int parsedTake)
				? parsedTake : 50;

			IEnumerable<GitCommit> commits = await _gitRunner.GetBranchHistoryAsync(repoPath, branchName, skip, take);

			// Response
			var response = new IpcMessage
			{
				Action = skip > 0 ? "BRANCH_HISTORY_APPENDED" : "BRANCH_HISTORY_LOADED",
				Payload = JsonSerializer.SerializeToElement(commits)
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
