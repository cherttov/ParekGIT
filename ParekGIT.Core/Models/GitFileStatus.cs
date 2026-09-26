namespace ParekGIT.Core.Models
{
	public record GitFileStatus
	{
		public required string StatusCode { get; init; }
		public required string Path { get; init; }
	}
}
