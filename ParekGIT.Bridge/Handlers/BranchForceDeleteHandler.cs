using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchForceDeleteHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitBranchService _branchService;

		public string Action => "BRANCH_FORCE_DELETE";

		// Constructor
		public BranchForceDeleteHandler(PhotinoWindow window, IGitBranchService branchService)
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

			await _branchService.ForceDeleteBranchAsync(repoPath, branchName);

			// Response
			var response = IpcMessage.Create("BRANCH_DELETED", new
			{
				repoPath,
				branchName
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
