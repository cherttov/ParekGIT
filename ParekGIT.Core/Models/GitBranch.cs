namespace ParekGIT.Core.Models
{
	// Convert to record?
	public class GitBranch
	{
		public string Name { get; set; } = string.Empty;
		public bool IsCurrent { get; set; } = false;
		public bool IsRemote { get; set; } = false;
		public string TargetCommitHash { get; set; } = string.Empty;
		public string RemoteBranch { get; set; } = string.Empty;
	}
}
