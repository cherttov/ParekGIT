using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
    public interface ITodoStore
    {
        Task<IEnumerable<TodoItem>> GetAllTodosAsync();
        Task UpsertTodoAsync(TodoItem todo);
        Task DeleteTodoAsync(Guid id);
    }
}
