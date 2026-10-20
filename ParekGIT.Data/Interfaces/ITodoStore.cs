using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface ITodoStore
	{
		Task<IEnumerable<TodoItem>> GetAllTodosAsync();
		Task<IEnumerable<TodoItem>> GetBranchTodosAsync(Guid repoId, string branchName);
		Task SaveBranchTodosAsync(Guid repoId, string branchName, IEnumerable<TodoItem> todos);
		Task UpsertTodoAsync(TodoItem todo);
		Task DeleteTodoAsync(Guid id);
	}
}
