using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchCreateHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "BRANCH_CREATE";

		// Constructor
		public BranchCreateHandler(PhotinoWindow window, IGitRunner gitRunner)
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

			// Create new branch
			await _gitRunner.CreateBranchAsync(repoPath, branchName);

			// Update displayed list
			IEnumerable<GitBranch> branches = await _gitRunner.GetBranchesAsync(repoPath);

			// Response
			var response = new IpcMessage
			{
				Action = "BRANCHES_LOADED", // maybe make a distinct "BRANCH_CREATED" action
				Payload = JsonSerializer.SerializeToElement(branches)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
