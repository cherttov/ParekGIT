using ParekGIT.Core.Interfaces;
using System.Reflection.Metadata;

namespace ParekGIT.Core.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IFileSystemService _fileSystem;
        private readonly ILogger _logger;
        private readonly string _gitIgnoreTemplatesDir;
        private readonly string _licenseTemplatesDir;

        // Constructor
        public TemplateService(IFileSystemService fileSystem, ILogger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;

            string baseDir = AppContext.BaseDirectory;
            _gitIgnoreTemplatesDir = Path.Combine(baseDir, "wwwroot", "templates", "gitignore");
            _licenseTemplatesDir = Path.Combine(baseDir, "wwwroot", "templates", "license");
        }

        public async Task WriteGitIgnoreAsync(string repoPath, string templateName)
        {
            await WriteTemplateFileAsync(repoPath, _gitIgnoreTemplatesDir, templateName, ".gitignore", false);
        }

        public async Task WriteLicenseAsync(string repoPath, string templateName, string year, string organization, string projectName)
        {
            await WriteTemplateFileAsync(repoPath, _licenseTemplatesDir, templateName, "LICENSE", true, organization, projectName);
        }

        // Helpers
        private async Task WriteTemplateFileAsync(string repoPath, string templatesDir, string templateName, string destFileName, 
            bool subPlaceholders, string? year = null, string? organization = null, string? projectName = null)
        {

            string templatePath = Path.Combine(templatesDir, $"{templateName}.txt");

            if (!_fileSystem.FileExists(templatePath))
            {
                throw new FileNotFoundException($"Template '{templateName}' not found.");
            }

            string content = await _fileSystem.ReadAllTextAsync(templatePath);

            // Replace license placeholders in LICENSE file
            if (subPlaceholders)
            {
                content = content
                    .Replace("{{ year }}", string.IsNullOrWhiteSpace(year) ? DateTime.Now.Year.ToString() : year)
                    .Replace("{{ organization }}", string.IsNullOrWhiteSpace(organization) ? "Unknown" : organization)
                    .Replace("{{ project }}", string.IsNullOrWhiteSpace(projectName) ? "Unknown" : projectName);

                if (content.Contains("{{") && content.Contains("}}"))
                {
                    await _logger.LogWarningAsync(
                        $"Template '{templateName}' may contain unahndled placeholders after substitution."
                    );
                }
            }

            string destinationPath = Path.Combine(repoPath, destFileName);
            await _fileSystem.WriteAllTextAsync(destinationPath, content);
        }
    }
}
