using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	public class TodoLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly LiteDbStore _dbStore;

		public string Action => "TODO_LOAD";

		// Constructor
		public TodoLoadHandler(PhotinoWindow window, LiteDbStore dbStore)
		{
			_window = window;
			_dbStore = dbStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			IEnumerable<GitRepository> allRepos = await _dbStore.GetAllRepositoriesAsync();
			GitRepository? targetRepo = allRepos.FirstOrDefault(repo => repo.AbsolutePath == repoPath);

			IEnumerable<TodoItem> todos = targetRepo != null
				? await _dbStore.GetRepoTodosAsync(targetRepo.Id)
				: throw new IpcPayloadException("repoPath", $"no repository found for path: {repoPath}");

			// Response
			var response = new IpcMessage
			{
				Action = "TODO_LOADED",
				Payload = JsonSerializer.SerializeToElement(new { todos = todos })
			};

			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
