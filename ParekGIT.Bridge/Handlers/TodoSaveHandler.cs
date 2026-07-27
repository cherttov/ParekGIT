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
	public class TodoSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly LiteDbStore _dbStore;

		public string Action => "TODO_SAVE";

		// Constructor
		public TodoSaveHandler(PhotinoWindow window, LiteDbStore dbStore)
		{
			_window = window;
			_dbStore = dbStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			// Get target repo
			IEnumerable<GitRepository> allRepos = await _dbStore.GetAllRepositoriesAsync();
			GitRepository targetRepo = allRepos.FirstOrDefault(repo => repo.AbsolutePath == repoPath)
				?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

			// Serialize into list
			JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			List<TodoItem> newTodos = payload.GetProperty("todos").Deserialize<List<TodoItem>>(options)
				?? new List<TodoItem>();

			// Assing repo ids & save
			foreach (var todo in newTodos) { todo.RepoId = targetRepo.Id; }
			await _dbStore.SaveRepoTodosAsync(targetRepo.Id, newTodos);

			// Response
			var response = new IpcMessage
			{
				Action = "TODO_SAVED",
				Payload = JsonSerializer.SerializeToElement("")
			};
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
