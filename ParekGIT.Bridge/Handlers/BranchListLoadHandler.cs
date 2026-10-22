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
		private readonly IGitRunner _gitRunner;

		public string Action => "BRANCH_LIST_LOAD";

		// Constructor
		public BranchListLoadHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			IEnumerable<GitBranch> branches = await _gitRunner.GetBranchesAsync(repoPath);

			// Response
			var response = new IpcMessage
			{
				Action = "BRANCH_LIST_LOADED",
				Payload = JsonSerializer.SerializeToElement(branches)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
