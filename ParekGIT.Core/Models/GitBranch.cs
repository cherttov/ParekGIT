namespace ParekGIT.Core.Models
{
    public class GitBranch
    {
        public string Name { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public bool IsRemote { get; set; }
        public string TargetCommitHash { get; set; } = string.Empty;
        public string RemoteBranch { get; set; } = string.Empty;
    }
}
