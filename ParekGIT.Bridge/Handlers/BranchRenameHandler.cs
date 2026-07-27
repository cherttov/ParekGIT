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
		private readonly IGitRunner _gitRunner;

		public string Action => "BRANCH_RENAME";

		// Constructor
		public BranchRenameHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string oldName = payload.GetProperty("oldName").GetString()
							  ?? throw new IpcPayloadException("oldName");

			string newName = payload.GetProperty("newName").GetString()
							  ?? throw new IpcPayloadException("newName");

			await _gitRunner.RenameBranchAsync(repoPath, oldName, newName);

			var responsePayload = new
			{
				repoPath = repoPath,
				oldName = oldName,
				newName = newName
			};

			// Response
			var response = new IpcMessage
			{
				Action = "BRANCH_RENAMED",
				Payload = JsonSerializer.SerializeToElement(responsePayload)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
