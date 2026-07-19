namespace ParekGIT.Core.Models
{
    public record CreateRepoRequest
    {
        public required string RepoName { get; init; }
        public required string LocalPath { get; init; }
        public string GitIgnore { get; init; } = "None";
        public string License { get; init; } = "None";
        public string? LicenseYear { get; init; } = null;
        public string? LicenseOrganization { get; init; } = null;
        public string? LicenseProject { get; init; } = null;
    }
}
