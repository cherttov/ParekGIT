using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchHistoryCreateHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "BRANCH_HISTORY_CREATE";

		// Constructor
		public BranchHistoryCreateHandler(PhotinoWindow window, IGitRunner gitRunner)
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

			string commitHash = payload.GetProperty("commitHash").GetString()
							  ?? throw new IpcPayloadException("commitHash");

			// Create new branch
			await _gitRunner.CreateBranchFromCommitAsync(repoPath, branchName, commitHash);

			// Update displayed list
			IEnumerable<GitBranch> branches = await _gitRunner.GetBranchesAsync(repoPath);

			// Response
			var response = new IpcMessage
			{
				Action = "BRANCHES_LOADED", // maybe make a distinct "BRANCH_HISTORY_CREATED" action
				Payload = JsonSerializer.SerializeToElement(branches)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
