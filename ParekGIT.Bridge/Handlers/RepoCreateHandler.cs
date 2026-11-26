using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoCreateHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _repoStore;
		private readonly IGitRepositoryService _repoService;

		public string Action => "REPO_CREATE";

		// Constructor
		public RepoCreateHandler(PhotinoWindow window, IRepositoryStore repoStore, IGitRepositoryService repoService)
		{
			_window = window;
			_repoStore = repoStore;
			_repoService = repoService;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			var request = payload.Deserialize<CreateRepoRequest>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
				?? throw new IpcPayloadException("Invalid or missing payload (CreateRepoRequest)");

			// Create repo in git
			var repo = await _repoService.CreateRepositoryAsync(request);
			await _repoStore.UpsertRepositoryAsync(repo);

			// Response
			var response = IpcMessage.Create("REPO_CREATED", new { repo });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
