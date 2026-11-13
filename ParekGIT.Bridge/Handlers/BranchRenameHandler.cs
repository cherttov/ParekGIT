using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchRenameHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitBranchService _branchService;

		public string Action => "BRANCH_RENAME";

		// Constructor
		public BranchRenameHandler(PhotinoWindow window, IGitBranchService branchService)
		{
			_window = window;
			_branchService = branchService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string oldName = payload.GetProperty("oldName").GetString()
							  ?? throw new IpcPayloadException("oldName");

			string newName = payload.GetProperty("newName").GetString()
							  ?? throw new IpcPayloadException("newName");

			await _branchService.RenameBranchAsync(repoPath, oldName, newName);

			// Response
			var response = IpcMessage.Create("BRANCH_RENAMED", new
			{
				repoPath,
				oldName,
				newName
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
