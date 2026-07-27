using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoCloneHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _dbStore;
		private readonly IGitRunner _gitRunner;

		public string Action => "REPO_CLONE";

		// Constructor
		public RepoCloneHandler(PhotinoWindow window, IRepositoryStore dbStore, IGitRunner gitRunner)
		{
			_window = window;
			_dbStore = dbStore;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoUrl = payload.GetProperty("repoUrl").GetString()
				?? throw new IpcPayloadException("repoUrl");

			bool asLocal = payload.GetProperty("asLocal").GetBoolean();

			string parentPath = payload.GetProperty("localPath").GetString()
				?? throw new IpcPayloadException("localPath");

			// Get repo name & combine with passed path
			string repoName = Path.GetFileNameWithoutExtension(repoUrl.TrimEnd('/').Split('/').Last());
			string localPath = Path.Combine(parentPath, repoName);

			await _gitRunner.CloneRepositoryAsync(repoUrl, asLocal, localPath);

			var clonedRepo = new GitRepository
			{
				Id = Guid.NewGuid(),
				Name = repoName,
				AbsolutePath = localPath,
				LastAccessed = DateTime.Now,
				IsRemote = !asLocal
			};

			await _dbStore.UpsertRepositoryAsync(clonedRepo);

			// Response
			var response = new IpcMessage
			{
				Action = "REPO_CLONED",
				Payload = JsonSerializer.SerializeToElement(clonedRepo)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
