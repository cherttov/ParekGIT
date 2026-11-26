using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoCloneHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _repoStore;
		private readonly IGitRepositoryService _repoService;

		public string Action => "REPO_CLONE";

		// Constructor
		public RepoCloneHandler(PhotinoWindow window, IRepositoryStore repoStore, IGitRepositoryService repoService)
		{
			_window = window;
			_repoStore = repoStore;
			_repoService = repoService;
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

			await _repoService.CloneRepositoryAsync(repoUrl, asLocal, localPath);

			var repo = new GitRepository
			{
				Id = Guid.NewGuid(),
				Name = repoName,
				AbsolutePath = localPath,
				LastAccessed = DateTime.Now,
				RemoteUrl = asLocal ? null : repoUrl
			};

			await _repoStore.UpsertRepositoryAsync(repo);

			// Response
			var response = IpcMessage.Create("REPO_CLONED", new { repo });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
