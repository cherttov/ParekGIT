using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using System.Globalization;

namespace ParekGIT.Core.Git
{
    public class GitCliRunner : IGitRunner
    {
        public async Task<string> ExecuteCommandAsync(string repositoryPath, string arguments)
        {
            try
            {
                var result = await Cli.Wrap("git")
                    .WithArguments(arguments)
                    .WithWorkingDirectory(repositoryPath)
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteBufferedAsync();

                if (result.ExitCode != 0)
                {
                    throw new Exception($"Git Error: {result.StandardError}");
                }

                return result.StandardOutput.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to run git command: {ex.Message}");
            }
        }

        // Parsers
        public async Task<IEnumerable<GitBranch>> GetBranchesAsync(string repositoryPath)
        {
            string arguments = "branch --all --format=\"%(refname:short)|%(HEAD)|%(upstream:short)|%(objectname)\"";

            string rawOutput = await ExecuteCommandAsync(repositoryPath, arguments);

            return GitBranchParser.Parse(rawOutput);
        }

        // Commands
        public async Task CheckoutBranchAsync(string repositoryPath, string branchName, bool isRemote)
        {
            string arguments = isRemote
                ? $"checkout -t \"{branchName}\""
                : $"checkout \"{branchName}\"";

            await ExecuteCommandAsync(repositoryPath, arguments);
        }

        public async Task CreateBranchAsync(string repositoryPath, string branchName)
        {
            string arguments = $"checkout -b \"{branchName}\"";

            await ExecuteCommandAsync(repositoryPath, arguments);
        }

        public async Task<GitRepository> CreateRepositoryAsync(string repoName, string description, string localPath, string gitIgnore, string license)
        {
            // Create & check directory
            string fullPath = Path.Combine(localPath, repoName);

            if (Directory.Exists(fullPath)) { throw new Exception($"Directory {repoName} already exists."); }

            Directory.CreateDirectory(fullPath);

            // Init repo
            await ExecuteCommandAsync(fullPath, "init -b main");

            // gitignore if not None
            if (!string.Equals(gitIgnore, "None", StringComparison.OrdinalIgnoreCase))
            {
                await GenerateGitIgnoreAsync(fullPath, gitIgnore);
            }

            // license if not None
            if (!string.Equals(license, "None", StringComparison.OrdinalIgnoreCase))
            {
                await GenerateLicenseAsync(fullPath, gitIgnore);
            }

            await ExecuteCommandAsync(fullPath, "add .");
            await ExecuteCommandAsync(fullPath, "commit -m \"Initial commit\"");

            return new GitRepository
            {
                Id = Guid.NewGuid(),
                Name = repoName,
                AbsolutePath = fullPath,
                LastAccessed = DateTime.UtcNow
            };
        }

        // Helpers
        private async Task GenerateGitIgnoreAsync(string path, string type)
        {
            // Define git ignores templates
            await File.WriteAllTextAsync(Path.Combine(path, ".gitignore"), "");
        }

        private async Task GenerateLicenseAsync(string path, string type)
        {
            // Define licenses
            await File.WriteAllTextAsync(Path.Combine(path, ".gitignore"), "");
        }
    }
}
