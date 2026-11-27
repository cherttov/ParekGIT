using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class BranchRenameHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitBranchService _branchService;
		private readonly ITodoStore _todoStore;

		public string Action => "BRANCH_RENAME";

		// Constructor
		public BranchRenameHandler(PhotinoWindow window, IGitBranchService branchService, ITodoStore todoStore)
		{
			_window = window;
			_branchService = branchService;
			_todoStore = todoStore;
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
			await _todoStore.RenameBranchTodosAsync(repoPath, oldName, newName);

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
