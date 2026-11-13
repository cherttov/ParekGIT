using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchListLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitBranchService _branchService;

		public string Action => "BRANCH_LIST_LOAD";

		// Constructor
		public BranchListLoadHandler(PhotinoWindow window, IGitBranchService branchService)
		{
			_window = window;
			_branchService = branchService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			IEnumerable<GitBranch> branches = await _branchService.GetBranchesAsync(repoPath);

			// Response
			var response = IpcMessage.Create("BRANCH_LIST_LOADED", new { branches });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
