namespace ParekGIT.Data.Models
{
	public class TodoItem
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid RepoId { get; set; }
		public string BranchName { get; set; } = string.Empty;
		public string TaskLabel { get; set; } = string.Empty;
		public bool IsCompleted { get; set; } = false;
	}
}
