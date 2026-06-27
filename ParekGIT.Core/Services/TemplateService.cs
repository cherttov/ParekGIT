using ParekGIT.Core.Interfaces;
using System.Reflection.Metadata;

namespace ParekGIT.Core.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IFileSystemService _fileSystem;
        private readonly string _gitIgnoreTemplatesDir;
        private readonly string _licenseTemplatesDir;

        // Constructor
        public TemplateService(IFileSystemService fileSystem)
        {
            _fileSystem = fileSystem;

            string baseDir = AppContext.BaseDirectory;
            _gitIgnoreTemplatesDir = Path.Combine(baseDir, "wwwroot", "templates", "gitignore");
            _licenseTemplatesDir = Path.Combine(baseDir, "wwwroot", "templates", "license");
        }

        public async Task WriteGitIgnoreAsync(string repoPath, string templateName)
        {
            await WriteTemplateFileAsync(repoPath, _gitIgnoreTemplatesDir, templateName, ".gitignore", false);
        }

        public async Task WriteLicenseAsync(string repoPath, string templateName, string? organization = null)
        {
            await WriteTemplateFileAsync(repoPath, _licenseTemplatesDir, templateName, "LICENSE", true, organization);
        }

        // Helpers
        private async Task WriteTemplateFileAsync(string repoPath, string templatesDir, string templateName, string destFileName, bool subPlaceholders, string? organization = null)
        {

            string templatePath = Path.Combine(templatesDir, $"{templateName}.txt");

            if (!_fileSystem.FileExists(templatePath))
            {
                throw new FileNotFoundException($"Template '{templateName}' not found.");
            }

            string content = await _fileSystem.ReadAllTextAsync(templatePath);

            // Replace license things in licenses
            if (subPlaceholders)
            {
                content = content
                    .Replace("{{ year }}", DateTime.Now.Year.ToString())
                    .Replace("{{ organization }}", string.IsNullOrWhiteSpace(organization) ? "Unknown" : organization);
            }

            string destinationPath = Path.Combine(repoPath, destFileName);

            await _fileSystem.WriteAllTextAsync(destinationPath, content);
        }
    }
}
