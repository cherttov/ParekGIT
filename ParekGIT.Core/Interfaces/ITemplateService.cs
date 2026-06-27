namespace ParekGIT.Core.Interfaces
{
    public interface ITemplateService
    {
        Task WriteGitIgnoreAsync(string repoPath, string templateName);
        Task WriteLicenseAsync(string repoPath, string templateName, string? organization = null, string? projectName = null);
    }
}
