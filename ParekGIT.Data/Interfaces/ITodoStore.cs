using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface ITodoStore
	{
		Task<IEnumerable<TodoItem>> GetBranchTodosAsync(string repoPath, string branchName);
		Task SaveBranchTodosAsync(string repoPath, string branchName, IEnumerable<TodoItem> todos);
		Task RenameBranchTodosAsync(string repoPath, string oldBranchName, string newBranchName);
		Task DeleteBranchTodosAsync(string repoPath, string branchName);
	}
}
