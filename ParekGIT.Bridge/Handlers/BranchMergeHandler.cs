using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class BranchMergeHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		public string Action => "BRANCH_MERGE";

		// Constructor
		public BranchMergeHandler(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string sourceBranch = payload.GetProperty("sourceBranch").GetString()
							  ?? throw new IpcPayloadException("sourceBranch");

			string targetBranch = payload.GetProperty("targetBranch").GetString()
							  ?? throw new IpcPayloadException("targetBranch");

			await _gitRunner.MergeBranchesAsync(repoPath, sourceBranch, targetBranch);

			// Response
			var response = new IpcMessage
			{
				Action = "BRANCH_MERGED",
				Payload = JsonSerializer.SerializeToElement(new { success = true })
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
