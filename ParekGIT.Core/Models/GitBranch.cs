namespace ParekGIT.Core.Models
{
	public record GitBranch
	{
		public required string Name { get; init; }
		public bool IsCurrent { get; init; } = false;
		public bool IsRemote { get; init; } = false;
		public required string TargetCommitHash { get; init; }
		public string RemoteBranch { get; init; } = string.Empty;
	}
}
