namespace ParekGIT.Core.Models
{
	public record GitConfigInfo
	{
		public string Name { get; init; } = string.Empty;
		public string Email { get; init; } = string.Empty;
	}
}
