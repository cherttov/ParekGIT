using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git
{
    public class GitCliRunner : IGitRunner
    {
        private readonly IFileSystemService _fileSystem;

        // Constructor
        public GitCliRunner(IFileSystemService fileSystem)
        {
            _fileSystem = fileSystem;
        }

        // Command executor
        public async Task<string> ExecuteCommandAsync(string? repoPath, string arguments)
        {
            string safeWorkingDir = string.IsNullOrWhiteSpace(repoPath)
                    ? Environment.CurrentDirectory
                    : repoPath;

            try
            {
                var result = await Cli.Wrap("git")
                    .WithArguments(arguments)
                    .WithWorkingDirectory(safeWorkingDir)
                    .WithValidation(CommandResultValidation.ZeroExitCode)
                    .ExecuteBufferedAsync();

                return result.StandardOutput.TrimEnd();
            }
            catch (Exception ex)
            {
                throw new Exception($"Git command failed: {ex.Message}");
            }
        }

        // Parsers
        public async Task<IEnumerable<GitBranch>> GetBranchesAsync(string repoPath)
        {
            string arguments = "branch --all --format=\"%(refname:short)|%(HEAD)|%(upstream:short)|%(objectname)\"";

            string rawOutput = await ExecuteCommandAsync(repoPath, arguments);

            return GitBranchParser.Parse(rawOutput);
        }

        public async Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repoPath)
        {
            string arguments = "status --porcelain -uall";

            string rawOutput = await ExecuteCommandAsync(repoPath, arguments);

            return GitStatusParser.Parse(rawOutput);
        }

        public async Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int limit = 50)
        {
            string arguments = $"log \"{branchName}\" -n {limit} --pretty=format:\"%H|%s|%an|%ar\"";

            string rawOutput = await ExecuteCommandAsync(repoPath, arguments);

            return GitHistoryParser.Parse(rawOutput);
        }

        public async Task<string> GetFileDiffAsync(string repoPath, string filePath)
        {
            string arguments = $"diff HEAD -- \"{filePath}\"";

            string rawOutput = await ExecuteCommandAsync(repoPath, arguments);

            // If empty, must be untracked file (read directly)
            if (string.IsNullOrWhiteSpace(rawOutput))
            {
                string fullPath = Path.Combine(repoPath, filePath);
                if (_fileSystem.FileExists(fullPath))
                {
                    string rawText = await _fileSystem.ReadAllTextAsync(fullPath);
                    var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    return string.Join("\n", lines.Select(line => "+" + line));
                }
            }

            return rawOutput;
        }

        public async Task<CommitDetailsResult>  GetCommitDetailsAsync(string repoPath, string hash)
        {
            string arguments = $"show --name-status --format=\"%an%n%s\" {hash}";

            string rawOutput = await ExecuteCommandAsync(repoPath, arguments);

            return GitCommitDetailsParser.Parse(rawOutput);
        }

        public async Task<string> GetHistoryFileDiffAsync(string repoPath, string commitHash, string filePath)
        {
            string arguments = $"show --format= {commitHash} -- \"{filePath}\"";

            return await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task<GitConfigInfo> GetGlobalConfigAsync(string? repoPath = null) 
        {
            string name = await GetConfigValueAsync(repoPath, "global", "user.name");
            string email = await GetConfigValueAsync(repoPath, "global", "user.email");

            return new GitConfigInfo
            {
                Name = name,
                Email = email
            };
        }

        public async Task<GitConfigInfo> GetLocalConfigAsync(string repoPath)
        {
            string name = await GetConfigValueAsync(repoPath, "local", "user.name");
            string email = await GetConfigValueAsync(repoPath, "local", "user.email");

            return new GitConfigInfo
            {
                Name = name,
                Email = email
            };
        }

        // Commands
        public async Task CheckoutBranchAsync(string repoPath, string branchName, bool isRemote)
        {
            string arguments = isRemote
                ? $"checkout -t \"{branchName}\""
                : $"checkout \"{branchName}\"";

            await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task CreateBranchAsync(string repoPath, string branchName)
        {
            string arguments = $"checkout -b \"{branchName}\"";

            await ExecuteCommandAsync(repoPath, arguments);
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

        public async Task RenameBranchAsync(string repoPath, string oldName, string newName)
        {
            string arguments = $"branch -m {oldName} {newName}";

            await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task DeleteBranchAsync(string repoPath, string branchName)
        {
            string arguments = $"branch -d {branchName}";

            await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task FetchRepositoryAsync(string repoPath)
        {
            string arguments = "fetch --all --prune";

            await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task DiscardChangeAsync(string repoPath, string filePath)
        {
            string statusArgs = $"status --porcelain -- \"{filePath}\"";
            string statusOutput = await ExecuteCommandAsync(repoPath, statusArgs);
            if (string.IsNullOrWhiteSpace(statusOutput)) { return; }

            string statusCode = statusOutput.Length >= 2 ? statusOutput.Substring(0, 2) : "";
            if (statusCode == "??")
            { 
                await ExecuteCommandAsync(repoPath, $"clean -f -- \"{filePath}\"");
            }
            else if (statusCode.StartsWith("A"))
            { 
                await ExecuteCommandAsync(repoPath, $"restore --staged -- \"{filePath}\"");
                await ExecuteCommandAsync(repoPath, $"clean -f -- \"{filePath}\"");
            }
            else // Modified/Deleted
            { 
                await ExecuteCommandAsync(repoPath, $"restore --staged --worktree -- \"{filePath}\"");
            }
        }

        public async Task IgnoreFileAsync(string repoPath, string filePath)
        {
            string gitignorePath = Path.Combine(repoPath, ".gitignore");
            string normalizedFilePath = filePath.Replace("\\", "/");

            bool alreadyIgnored = false;
            if (File.Exists(gitignorePath))
            {
                var lines = await File.ReadAllLinesAsync(gitignorePath);
                alreadyIgnored = lines.Any(lines => lines.Trim() == normalizedFilePath);
            }

            if (!alreadyIgnored)
            {
                string contentToAppend = File.Exists(gitignorePath) && new FileInfo(gitignorePath).Length > 0
                    ? Environment.NewLine + normalizedFilePath
                    : normalizedFilePath;

                await File.AppendAllTextAsync(gitignorePath, contentToAppend + Environment.NewLine);
            }

            string arguments = $"rm --cached --ignore-unmatch -- \"{filePath}\"";

            await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task IgnoreFolderAsync(string repoPath, string filePath) 
        {
            await Task.CompletedTask;
        }

        public async Task IgnoreExtensionAsync(string repoPath, string filePath) 
        {
            await Task.CompletedTask;
        }

        public async Task CheckoutCommitAsync(string repoPath, string commitHash)
        {
            var status = await GetStatusAsync(repoPath);

            if (status.Any())
            {
                throw new Exception("Uncommitted changes are present [CheckoutCommitAsync]");
            }

            string arguments = $"checkout --detach {commitHash}";

            await ExecuteCommandAsync(repoPath, arguments);
        }

        public async Task RevertCommitAsync(string repoPath, string commitHash)
        {
            var status = await GetStatusAsync(repoPath);
            if (status.Any())
            {
                throw new Exception("Uncommitted changes are present [RevertCommitAsync]");
            }

            try
            {
                // Revert
                string arguments = $"revert --no-edit {commitHash}";

                await ExecuteCommandAsync(repoPath, arguments);
            }
            catch (Exception ex)
            {
                // Merge conflict
                if (ex.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("could not revert", StringComparison.OrdinalIgnoreCase))
                {
                    await ExecuteCommandAsync(repoPath, "revert --abort");

                    throw new Exception("Merge conflict while reverting changes [RevertCommitAsync]");
                }

                // Random errors
                throw;
            }
        }

        public async Task MergeBranchesAsync(string repoPath, string sourceBranch, string targetBranch)
        {
            var status = await GetStatusAsync(repoPath);
            if (status.Any())
            {
                throw new Exception("Uncommitted changes are present [MergeBranchesAsync]");
            }

            await ExecuteCommandAsync(repoPath, $"checkout \"{targetBranch}\"");

            try
            {
                await ExecuteCommandAsync(repoPath, $"merge \"{sourceBranch}\"");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("merge failed", StringComparison.OrdinalIgnoreCase))
                {
                    await ExecuteCommandAsync(repoPath, "merge --abort");

                    throw new Exception($"Merge conflict occured while merging '{sourceBranch}' -> '{targetBranch}'. Merge aborted.");
                }

                throw;
            }
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
                await GenerateLicenseAsync(fullPath, license);
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

        public async Task SaveLocalConfigAsync(string repoPath, string name, string email)
        {
            // Handle Name
            if (string.IsNullOrWhiteSpace(name))
            {
                await ExecuteCommandAsync(repoPath, "config --local --unset user.name");
            }
            else
            {
                string safeName = name.Replace("\"", "\\\"");
                await ExecuteCommandAsync(repoPath, $"config --local user.name \"{safeName}\"");
            }

            // Handle Email
            if (string.IsNullOrWhiteSpace(email))
            {
                await ExecuteCommandAsync(repoPath, "config --local --unset user.email");
            }
            else
            {
                string safeEmail = email.Replace("\"", "\\\"");
                await ExecuteCommandAsync(repoPath, $"config --local user.email \"{safeEmail}\"");
            }
        }

        public async Task SaveGlobalConfigAsync(string? repoPath, string name, string email)
        {
            // Handle Name
            if (string.IsNullOrWhiteSpace(name))
            {
                await ExecuteCommandAsync(repoPath, "config --global --unset user.name");
            }
            else
            {
                string safeName = name.Replace("\"", "\\\"");
                await ExecuteCommandAsync(repoPath, $"config --global user.name \"{safeName}\"");
            }

            // Handle Email
            if (string.IsNullOrWhiteSpace(email))
            {
                await ExecuteCommandAsync(repoPath, "config --global --unset user.email");
            }
            else
            {
                string safeEmail = email.Replace("\"", "\\\"");
                await ExecuteCommandAsync(repoPath, $"config --global user.email \"{safeEmail}\"");
            }
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

        private async Task<string> GetConfigValueAsync(string? repoPath, string scope, string key)
        {
            string safeWorkingDir = string.IsNullOrWhiteSpace(repoPath)
                ? Environment.CurrentDirectory
                : repoPath;

            var result = await Cli.Wrap("git")
                .WithArguments($"config --{scope} {key}")
                .WithWorkingDirectory(safeWorkingDir)
                .WithValidation(CommandResultValidation.None)
                .ExecuteBufferedAsync();

            if (result.ExitCode == 1 && string.IsNullOrWhiteSpace(result.StandardError))
            {
                return string.Empty;
            }

            if (result.ExitCode != 0)
            {
                string stderr = result.StandardError.Trim();
                throw new Exception($"Git command failed: exit {result.ExitCode}");
            }

            return result.StandardOutput.TrimEnd();
        }
    }
}
