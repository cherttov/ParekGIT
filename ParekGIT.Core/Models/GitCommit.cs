namespace ParekGIT.Core.Models
{
	public record GitCommit
	{
		public required string Hash { get; init; }
		public required string Message { get; init; }
		public required string Author { get; init; }
		public required string TimeAgo { get; init; }
		public required DateTime CommitDate { get; init; }
	}
}
