namespace ParekGIT.Core.Models
{
	// Convert to record
	public class CommitDetailsResult
	{
		public string Author { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public List<GitFileStatus> Files { get; set; } = new();
	}
}
