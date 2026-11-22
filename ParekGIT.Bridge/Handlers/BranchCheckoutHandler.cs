using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchCheckoutHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitBranchService _branchService;

		public string Action => "BRANCH_CHECKOUT";

		// Constructor
		public BranchCheckoutHandler(PhotinoWindow window, IGitBranchService branchService)
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

			bool isRemote = payload.GetProperty("isRemote").GetBoolean();

			// Change branch in git
			await _branchService.CheckoutBranchAsync(repoPath, branchName, isRemote);

			// Update displayed list
			IEnumerable<GitBranch> branches = await _branchService.GetBranchesAsync(repoPath);
			GitBranch branch = branches.FirstOrDefault(b => b.IsCurrent == true)!;

			// Response
			var response = IpcMessage.Create("BRANCH_CHECKED_OUT", new
			{
				branch,
				branches
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
