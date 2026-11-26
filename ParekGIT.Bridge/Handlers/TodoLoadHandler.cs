using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class TodoLoadHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly ITodoStore _todoStore;

		public string Action => "TODO_LOAD";

		// Constructor
		public TodoLoadHandler(PhotinoWindow window, ITodoStore todoStore)
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

			IEnumerable<TodoItem> todos = await _todoStore.GetBranchTodosAsync(repoPath, branchName);

			// Response
			var response = IpcMessage.Create("TODO_LOADED", new { todos });
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
