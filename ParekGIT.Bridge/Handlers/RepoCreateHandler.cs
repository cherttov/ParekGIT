using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class RepoCreateHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _dbStore;
		private readonly IGitRunner _gitRunner;

		public string Action => "REPO_CREATE";

		// Constructor
		public RepoCreateHandler(PhotinoWindow window, IRepositoryStore dbStore, IGitRunner gitRunner)
		{
			_window = window;
			_dbStore = dbStore;
			_gitRunner = gitRunner;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			var request = payload.Deserialize<CreateRepoRequest>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
				?? throw new IpcPayloadException("Invalid or missing payload (CreateRepoRequest)");

			// Create repo in git
			var repo = await _gitRunner.CreateRepositoryAsync(request);
			await _dbStore.UpsertRepositoryAsync(repo);

			// Response
			var response = IpcMessage.Create("REPO_CREATED", new { repo });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
