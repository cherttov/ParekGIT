using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class BranchForceDeleteHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitBranchService _branchService;
		private readonly ITodoStore _todoStore;

		public string Action => "BRANCH_FORCE_DELETE";

		// Constructor
		public BranchForceDeleteHandler(PhotinoWindow window, IGitBranchService branchService, ITodoStore todoStore)
		{
			_window = window;
			_branchService = branchService;
			_todoStore = todoStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string branchName = payload.GetProperty("branchName").GetString()
							  ?? throw new IpcPayloadException("branchName");

			await _branchService.ForceDeleteBranchAsync(repoPath, branchName);
			await _todoStore.DeleteBranchTodosAsync(repoPath, branchName);

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
