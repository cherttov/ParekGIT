using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

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

                return result.StandardOutput.TrimEnd();
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

        public async Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repositoryPath)
        {
            string arguments = "status --porcelain";

            string rawOutput = await ExecuteCommandAsync(repositoryPath, arguments);

            return GitStatusParser.Parse(rawOutput);
        }

        public async Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repositoryPath, string branchName, int limit = 50)
        {
            string arguments = $"log \"{branchName}\" -n {limit} --pretty=format:\"%H|%s|%an|%ar\"";

            string rawOutput = await ExecuteCommandAsync(repositoryPath, arguments);

            return GitHistoryParser.Parse(rawOutput);
        }

        public async Task<string> GetFileDiffAsync(string repositoryPath, string filePath)
        {
            string arguments = $"diff HEAD -- \"{filePath}\"";

            string rawOutput = await ExecuteCommandAsync(repositoryPath, arguments);

            // If empty, must be untracked file (read directly)
            if (string.IsNullOrWhiteSpace(rawOutput))
            {
                string fullPath = Path.Combine(repositoryPath, filePath);
                if (File.Exists(fullPath))
                {
                    string rawText = await File.ReadAllTextAsync(fullPath);

                    var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    return string.Join("\n", lines.Select(line => "+" + line));
                }
            }

            return rawOutput;
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

        public async Task<GitRepository> CreateRepositoryAsync(string repoName, string localPath, string gitIgnore, string license)
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

        public async Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files)
        {
            if (files == null) { return; }

            // Unstage everything
            await ExecuteCommandAsync(repoPath, "reset");

            // Stage selected files
            string fileArgs = string.Join(" ", files.Select(file => $"\"{file}\""));
            await ExecuteCommandAsync(repoPath, $"add -- {fileArgs}");

            // Prepare commit command
            string safeMessage = message.Replace("\"", "\\\"");
            string commitArgs = $"commit -m \"{safeMessage}\"";

            // Add description if typed in
            if (!string.IsNullOrWhiteSpace(desc))
            {
                string safeDescription = desc.Replace("\"", "\\\"");
                commitArgs += $" -m \"{safeDescription}\"";
            }

            await ExecuteCommandAsync(repoPath, commitArgs);
        }

        public async Task RenameBranchAsync(string repositoryPath, string oldName, string newName)
        {
            string arguments = $"branch -m {oldName} {newName}";

            await ExecuteCommandAsync(repositoryPath, arguments);
        }

        public async Task DeleteBranchAsync(string repositoryPath, string branchName)
        {
            string arguments = $"branch -d {branchName}";

            await ExecuteCommandAsync(repositoryPath, arguments);
        }

        public async Task FetchRepositoryAsync(string repositoryPath)
        {
            string arguments = "fetch --all --prune";

            await ExecuteCommandAsync(repositoryPath, arguments);
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
