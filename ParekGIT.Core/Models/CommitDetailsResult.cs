namespace ParekGIT.Core.Models
{
	public record CommitDetailsResult
	{
		public required string Author { get; init; }
		public required string Message { get; init; }
		public List<GitFileStatus> Files { get; init; } = new();
	}
}
