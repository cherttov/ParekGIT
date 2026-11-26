using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface ITodoStore
	{
		Task<IEnumerable<TodoItem>> GetAllTodosAsync();
		Task<IEnumerable<TodoItem>> GetBranchTodosAsync(string repoPath, string branchName);
		Task SaveBranchTodosAsync(string repoPath, string branchName, IEnumerable<TodoItem> todos);
		Task UpsertTodoAsync(TodoItem todo);
		Task DeleteTodoAsync(Guid id);
	}
}
