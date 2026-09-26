namespace ParekGIT.Core.Models
{
	public record GitStash
	{
		public required string Selector { get; init; }
		public required string CommitHash { get; init; }
		public required string Message { get; init; }
		public string BranchName { get; init; } = string.Empty;
	}
}
