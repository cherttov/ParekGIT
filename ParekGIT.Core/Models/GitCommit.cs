namespace ParekGIT.Core.Models
{
	// Convert to record
	public class GitCommit
	{
		public string Hash { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public string TimeAgo { get; set; } = string.Empty; // Change to DateTime
	}
}
