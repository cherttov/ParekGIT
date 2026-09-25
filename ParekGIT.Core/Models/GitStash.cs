namespace ParekGIT.Core.Models
{
	// Convert to record
	public class GitStash
	{
		public string Selector { get; set; } = string.Empty;
		public string CommitHash { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public string BranchName { get; set; } = string.Empty;
	}
}
