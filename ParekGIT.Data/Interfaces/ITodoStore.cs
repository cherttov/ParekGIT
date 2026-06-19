using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
    public interface ITodoStore
    {
        Task<IEnumerable<TodoItem>> GetAllTodosAsync();
        Task<IEnumerable<TodoItem>> GetRepoTodosAsync(Guid repoId);
        Task SaveRepoTodosAsync(Guid repoId, IEnumerable<TodoItem> todos);
        Task UpsertTodoAsync(TodoItem todo);
        Task DeleteTodoAsync(Guid id);
    }
}
