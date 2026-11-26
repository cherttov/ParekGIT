using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class TodoSaveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly ITodoStore _todoStore;

		public string Action => "TODO_SAVE";

		// Constructor
		public TodoSaveHandler(PhotinoWindow window, ITodoStore todoStore)
		{
			_window = window;
			_todoStore = todoStore;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
							  ?? throw new IpcPayloadException("repoPath");

			string branchName = payload.GetProperty("branchName").GetString()
								?? throw new IpcPayloadException("branchName");

			JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			List<TodoItem> newTodos = payload.GetProperty("todos").Deserialize<List<TodoItem>>(options)
				?? new List<TodoItem>();

			await _todoStore.SaveBranchTodosAsync(repoPath, branchName, newTodos);

			// Response
			var response = IpcMessage.Create("TODO_SAVED", new { success = true });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
