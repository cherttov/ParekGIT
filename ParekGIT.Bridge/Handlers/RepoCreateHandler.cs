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
				?? throw new IpcPayloadException("Invalid or missing payload (CreatRepoRequest)");

			// Create repo in git
			var repository = await _gitRunner.CreateRepositoryAsync(request);
			await _dbStore.UpsertRepositoryAsync(repository);

			// Response
			var response = new IpcMessage
			{
				Action = "REPO_CREATED",
				Payload = JsonSerializer.SerializeToElement(repository)
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
