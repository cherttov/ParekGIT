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
		private readonly IGitBranchService _branchService;

		public string Action => "BRANCH_CREATE";

		// Constructor
		public BranchCreateHandler(PhotinoWindow window, IGitBranchService branchService)
		{
			_window = window;
			_branchService = branchService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string branchName = payload.GetProperty("branchName").GetString()
								?? throw new IpcPayloadException("branchName");

			// Create new branch
			await _branchService.CreateBranchAsync(repoPath, branchName);

			// Update displayed list
			IEnumerable<GitBranch> branches = await _branchService.GetBranchesAsync(repoPath);

			// Response
			var response = IpcMessage.Create("BRANCH_LIST_LOADED", new { branches }); // maybe make a distinct "BRANCH_CREATED" action
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
