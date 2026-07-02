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
            string templatePath = Path.Combine(_gitIgnoreTemplatesDir, $"{templateName}.txt");

            if (!_fileSystem.FileExists(templatePath))
            {
                throw new FileNotFoundException($"GitIgnore template '{templateName}' not found.");
            }

            string content = await _fileSystem.ReadAllTextAsync(templatePath);

            string destinationPath = Path.Combine(repoPath, ".gitignore");
            await _fileSystem.WriteAllTextAsync(destinationPath, content);
        }

        public async Task WriteLicenseAsync(string repoPath, string templateName, string year, string organization, string project)
        {
            string templatePath = Path.Combine(_licenseTemplatesDir, $"{templateName}.txt");

            if (!_fileSystem.FileExists(templatePath))
            {
                throw new FileNotFoundException($"License template '{templateName}' not found.");
            }

            string content = await _fileSystem.ReadAllTextAsync(templatePath);

            // Replace placeholders
            content = content
                    .Replace("{{ year }}", string.IsNullOrWhiteSpace(year) ? DateTime.Now.Year.ToString() : year)
                    .Replace("{{ organization }}", string.IsNullOrWhiteSpace(organization) ? "Unknown Author" : organization)
                    .Replace("{{ project }}", string.IsNullOrWhiteSpace(project) ? "Unknown Project" : project);

            // Warn about unresolved placeholders
            if (content.Contains("{{") && content.Contains("}}"))
            {
                await _logger.LogWarningAsync(
                    $"Template '{templateName}' may contain unahndled placeholders after substitution."
                );
            }

            string destinationPath = Path.Combine(repoPath, "LICENSE");
            await _fileSystem.WriteAllTextAsync(destinationPath, content);
        }
    }
}
