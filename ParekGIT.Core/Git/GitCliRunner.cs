using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using System.Runtime.CompilerServices;

namespace ParekGIT.Core.Git
{
	public class GitCliRunner : IGitRunner
	{
		private readonly IFileSystemService _fileSystem;
		private readonly ITemplateService _templateService;
		private readonly ILogger _logger;

		// Constructor
		public GitCliRunner(IFileSystemService fileSystem, ITemplateService templateService, ILogger logger)
		{
			_fileSystem = fileSystem;
			_templateService = templateService;
			_logger = logger;
		}

		// ======================== Command execution ========================
		public async Task<string> ExecuteCommandAsync(string? repoPath, IEnumerable<string> args, params int[] successExitCodes)
		{
			string safeWorkingDir = string.IsNullOrWhiteSpace(repoPath)
					? Environment.CurrentDirectory
					: repoPath;

			if (!Directory.Exists(safeWorkingDir))
			{
				throw new Exception($"Git command failed: working directory does not exist: '{safeWorkingDir}'");
			}

			try
			{
				var cmd = Cli.Wrap("git")
					.WithArguments(args)
					.WithWorkingDirectory(safeWorkingDir)
					.WithValidation(CommandResultValidation.None);

				var result = await cmd.ExecuteBufferedAsync();

				bool isAcceptable = result.ExitCode == 0 || successExitCodes.Contains(result.ExitCode);

				if (!isAcceptable)
				{
					throw new Exception($"Git command failed: exit {result.ExitCode}. {result.StandardError.Trim()}");
				}

				// Just extra log
				if (result.ExitCode != 0)
				{
					await _logger.LogWarningAsync(
						$"{cmd} exited {result.ExitCode} (acceptable). {result.StandardError.Trim()}"
					);
				}

				return result.StandardOutput.TrimEnd();
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				if (ex.Message.StartsWith("Git command failed")) throw; 
				throw new Exception($"Git command failed: {ex.Message}", ex);
			}
		}

		// ======================== Parsers ========================
		public async Task<IEnumerable<GitBranch>> GetBranchesAsync(string repoPath) =>
			GitBranchParser.Parse(await ExecuteCommandAsync(repoPath, [
				"branch", "--all", "--format=%(refname)|%(refname:short)|%(HEAD)|%(upstream:short)|%(objectname)"
			]));

		public async Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repoPath) =>
			GitStatusParser.Parse(await ExecuteCommandAsync(repoPath, ["status", "--porcelain", "-uall"]));

		public async Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int skip = 0, int take = 50) =>
			GitHistoryParser.Parse(await ExecuteCommandAsync(repoPath, [
				"log", branchName, "--skip", skip.ToString(), "-n", take.ToString(), "--pretty=format:%H|%s|%an|%ar|%at"
			]));

		public async Task<IEnumerable<GitStash>> GetStashesAsync(string repoPath) =>
			GitStashParser.Parse(await ExecuteCommandAsync(repoPath, ["stash", "list", "--format=%gd|%H|%s"]));

		public async Task<string> GetFileDiffAsync(string repoPath, string filePath)
		{
			string[] args = ["diff", "HEAD", "--", filePath];

			string rawOutput = await ExecuteCommandAsync(repoPath, args);

			// If empty, must be untracked file (read directly)
			if (string.IsNullOrWhiteSpace(rawOutput))
			{
				string fullPath = Path.Combine(repoPath, filePath);
				if (_fileSystem.FileExists(fullPath))
				{
					// Add binary check here
					string rawText = await _fileSystem.ReadAllTextAsync(fullPath);
					var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
					return string.Join("\n", lines.Select(line => "+" + line));
				}
			}

			return rawOutput;
		}

		public async Task<CommitDetailsResult> GetCommitDetailsAsync(string repoPath, string hash) =>
			GitCommitDetailsParser.Parse(await ExecuteCommandAsync(repoPath, [
				"show", "--diff-merges=first-parent", "--name-status", "--format=%an%n%s", hash
			]));

		public async Task<int> GetCommitsAheadAsync(string repoPath) =>
			await GetRevListCountAsync(repoPath, "@{u}..HEAD");


		public async Task<int> GetCommitsBehindAsync(string repoPath) =>
			await GetRevListCountAsync(repoPath, "HEAD..@{u}");

		public async Task<string> GetHistoryFileDiffAsync(string repoPath, string commitHash, string filePath) =>
			await ExecuteCommandAsync(repoPath, ["show", "--diff-merges=first-parent", "--format=", commitHash, "--", filePath]);

		public async Task<GitConfigInfo> GetGlobalConfigAsync(string? repoPath = null) =>
			await GetConfigInternalAsync(repoPath, "--global");


		public async Task<GitConfigInfo> GetLocalConfigAsync(string repoPath) =>
			await GetConfigInternalAsync(repoPath, "--local");

		public async Task<string?> GetRemoteUrlAsync(string repoPath)
		{
			string rawOutput = await ExecuteCommandAsync(repoPath, ["config", "--get", "remote.origin.url"], 1);

			return string.IsNullOrWhiteSpace(rawOutput) ? null : rawOutput;
		}

		// ======================== Commands ========================
		public async Task CheckoutBranchAsync(string repoPath, string branchName, bool isRemote)
		{
			string[] args = isRemote
				? ["checkout", "-t", branchName]
				: ["checkout", branchName];

			await ExecuteCommandAsync(repoPath, args);
		}

		public async Task CreateBranchAsync(string repoPath, string branchName) =>
			await ExecuteCommandAsync(repoPath, ["checkout", "-b", branchName]);

		public async Task CreateBranchFromCommitAsync(string repoPath, string branchName, string commitHash) =>
			await ExecuteCommandAsync(repoPath, ["checkout", "-b", branchName, commitHash]);

		public async Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files)
		{
			if (files == null || !files.Any()) return;

			// Unstage everything
			await ExecuteCommandAsync(repoPath, ["reset"]);

			// Stage selected files
			await ExecuteCommandAsync(repoPath, ["add", "--", ..files]);

			// Prepare commit command
			List<string> commitArgs = ["commit", "-m", message];

			// Add description if typed in
			if (!string.IsNullOrWhiteSpace(desc))
				commitArgs.AddRange(["-m", desc]);

			await ExecuteCommandAsync(repoPath, commitArgs);
		}

		public async Task PushAsync(string repoPath) =>
			await ExecuteCommandAsync(repoPath, ["push", "-u", "origin", "HEAD"]);

		public async Task PullAsync(string repoPath) =>
			await ExecuteCommandAsync(repoPath, ["pull"]);

		public async Task RenameBranchAsync(string repoPath, string oldName, string newName) =>
			await ExecuteCommandAsync(repoPath, ["branch", "-m", oldName, newName]);

		public async Task DeleteBranchAsync(string repoPath, string branchName) =>
			await ExecuteCommandAsync(repoPath, ["branch", "-d", branchName]);

		public async Task ForceDeleteBranchAsync(string repoPath, string branchName) =>
			await ExecuteCommandAsync(repoPath, ["branch", "-D", branchName]);

		public async Task DeleteRemoteBranchAsync(string repoPath, string remoteName, string branchName) =>
			await ExecuteCommandAsync(repoPath, ["push", remoteName, "-d", branchName]);

		public async Task FetchRepositoryAsync(string repoPath) =>
			await ExecuteCommandAsync(repoPath, ["fetch", "--all", "--prune"]);

		public async Task DiscardChangeAsync(string repoPath, string filePath)
		{
			string[] statusArgs = ["status", "--porcelain", "--", filePath];
			string statusOutput = await ExecuteCommandAsync(repoPath, statusArgs);
			if (string.IsNullOrWhiteSpace(statusOutput)) return;

			string statusCode = statusOutput.Length >= 2 ? statusOutput.Substring(0, 2) : "";
			if (statusCode == "??")
			{
				await ExecuteCommandAsync(repoPath, ["clean", "-f", "--", filePath]);
			}
			else if (statusCode.StartsWith("A"))
			{
				await ExecuteCommandAsync(repoPath, ["restore", "--staged", "--", filePath]);
				await ExecuteCommandAsync(repoPath, ["clean", "-f", "--", filePath]);
			}
			else // Modified/Deleted
			{
				await ExecuteCommandAsync(repoPath, ["restore", "--staged", "--worktree", "--", filePath]);
			}
		}

		public async Task DiscardAllChangesAsync(string repoPath)
		{
			await ExecuteCommandAsync(repoPath, ["reset", "--hard", "HEAD"]);
			await ExecuteCommandAsync(repoPath, ["clean", "-fd"]);
		}

		public async Task IgnoreFileAsync(string repoPath, string filePath)
		{
			string gitignorePath = Path.Combine(repoPath, ".gitignore");
			string normalizedFilePath = filePath.Replace("\\", "/");

			bool alreadyIgnored = false;
			if (_fileSystem.FileExists(gitignorePath))
			{
				var lines = await _fileSystem.ReadAllLinesAsync(gitignorePath);
				alreadyIgnored = lines.Any(line => line.Trim() == normalizedFilePath);
			}

			if (!alreadyIgnored)
			{
				string contentToAppend = _fileSystem.FileExists(gitignorePath) && new FileInfo(gitignorePath).Length > 0
					? Environment.NewLine + normalizedFilePath
					: normalizedFilePath;

				await _fileSystem.AppendAllTextAsync(gitignorePath, contentToAppend + Environment.NewLine);
			}

			await ExecuteCommandAsync(repoPath, ["rm", "--cached", "--ignore-unmatch", "--", filePath]);
		}

		public Task IgnoreFolderAsync(string repoPath, string filePath)
		{
			throw new NotImplementedException();
		}

		public Task IgnoreExtensionAsync(string repoPath, string filePath)
		{
			throw new NotImplementedException();
		}

		public async Task CheckoutCommitAsync(string repoPath, string commitHash)
		{
			await EnsureCleanWorkingTreeAsync(repoPath);
			await ExecuteCommandAsync(repoPath, ["checkout", "--detach", commitHash]);
		}

		public async Task RevertCommitAsync(string repoPath, string commitHash)
		{
			await EnsureCleanWorkingTreeAsync(repoPath);

			try
			{
				// Revert
				await ExecuteCommandAsync(repoPath, ["revert", "--no-edit", commitHash]);
			}
			catch (Exception ex) when (
				ex.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
				ex.Message.Contains("could not revert", StringComparison.OrdinalIgnoreCase))
			{
				// Merge conflict
				await _logger.LogWarningAsync(
					$"Revert conflict on commit '{commitHash}' in '{repoPath}'. Aborting. Git message: {ex.Message}"
				);

				await ExecuteCommandAsync(repoPath, ["revert", "--abort"]);

				throw new Exception("Merge conflict while reverting changes [RevertCommitAsync]");
			}
		}

		public async Task MergeBranchesAsync(string repoPath, string sourceBranch, string targetBranch)
		{
			await EnsureCleanWorkingTreeAsync(repoPath);
			await ExecuteCommandAsync(repoPath, ["checkout", targetBranch]);

			try
			{
				await ExecuteCommandAsync(repoPath, ["merge", sourceBranch]);
			}
			catch (Exception ex) when (
				ex.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
				ex.Message.Contains("merge failed", StringComparison.OrdinalIgnoreCase))
			{
				await _logger.LogWarningAsync(
					$"Merge conflict while merging '{sourceBranch}' -> '{targetBranch}'. Aborting. Git message: {ex.Message}"
				);

				await ExecuteCommandAsync(repoPath, ["merge", "--abort"]);

				throw new Exception($"Merge conflict occurred while merging '{sourceBranch}' -> '{targetBranch}'. Merge aborted.");
			}
		}

		public async Task<GitRepository> CreateRepositoryAsync(CreateRepoRequest request)
		{
			// Create & check directory
			string fullPath = Path.Combine(request.LocalPath, request.RepoName);
			if (Directory.Exists(fullPath)) 
				throw new Exception($"Directory {request.RepoName} already exists.");

			// Init repo
			Directory.CreateDirectory(fullPath);
			await ExecuteCommandAsync(fullPath, ["init", "-b", "main"]);

			// gitignore if not None
			if (!string.Equals(request.GitIgnore, "None", StringComparison.OrdinalIgnoreCase))
				await _templateService.WriteGitIgnoreAsync(fullPath, request.GitIgnore);

			// license if not None
			if (!string.Equals(request.License, "None", StringComparison.OrdinalIgnoreCase))
			{
				GitConfigInfo globalConfig = await GetGlobalConfigAsync(null);

				string year = string.IsNullOrWhiteSpace(request.LicenseYear) ? DateTime.Now.Year.ToString() : request.LicenseYear;
				string org = string.IsNullOrWhiteSpace(request.LicenseOrganization) ? globalConfig.Name : request.LicenseOrganization;
				string proj = string.IsNullOrWhiteSpace(request.LicenseProject) ? request.RepoName : request.LicenseProject;

				await _templateService.WriteLicenseAsync(fullPath, request.License, year, org, proj);
			}

			await ExecuteCommandAsync(fullPath, ["add", "."]);
			await ExecuteCommandAsync(fullPath, ["commit", "--allow-empty", "-m", "Initial commit"]);

			return new GitRepository
			{
				Id = Guid.NewGuid(),
				Name = request.RepoName,
				AbsolutePath = fullPath,
				LastAccessed = DateTime.UtcNow
			};
		}

		public async Task CloneRepositoryAsync(string repoUrl, bool asLocal, string localPath)
		{
			await ExecuteCommandAsync(null, ["clone", repoUrl, localPath]);

			if (asLocal)
				await ExecuteCommandAsync(localPath, ["remote", "remove", "origin"]);
		}

		public async Task SaveLocalConfigAsync(string repoPath, string name, string email) =>
			await SaveConfigInternalAsync(repoPath, "--local", name, email);

		public async Task SaveGlobalConfigAsync(string? repoPath, string name, string email) =>
			await SaveConfigInternalAsync(repoPath, "--global", name, email);

		public async Task StashSaveAsync(string repoPath, string message, IEnumerable<string> files)
		{
			List<string> args = ["stash", "push", "--include-untracked"];

			if (!string.IsNullOrWhiteSpace(message))
				args.AddRange(["-m", message]);

			if (files?.Any() == true)
				args.AddRange(["--", .. files]);

			await ExecuteCommandAsync(repoPath, args);
		}

		public async Task StashPopAsync(string repoPath, string stashSelector) =>
			await ExecuteCommandAsync(repoPath, ["stash", "pop", stashSelector]);

		public async Task StashDropAsync(string repoPath, string stashSelector) =>
			await ExecuteCommandAsync(repoPath, ["stash", "drop", stashSelector]);

		// ======================== Helpers ========================
		private async Task<GitConfigInfo> GetConfigInternalAsync(string? repoPath, string scopeFlag)
		{
			string[] args = ["config", scopeFlag, "--get-regexp", @"^user\.(name|email)$"];
			return GitConfigParser.Parse(await ExecuteCommandAsync(repoPath, args, 1));
		}
		
		private async Task SaveConfigInternalAsync(string? repoPath, string scopeFlag, string name, string email)
		{
			// Name
			if (string.IsNullOrWhiteSpace(name))
			{
				await ExecuteCommandAsync(repoPath, ["config", scopeFlag, "--unset", "user.name"], successExitCodes: 5);
			}
			else
			{
				await ExecuteCommandAsync(repoPath, ["config", scopeFlag, "user.name", name]);
			}

			// Email
			if (string.IsNullOrWhiteSpace(email))
			{
				await ExecuteCommandAsync(repoPath, ["config", scopeFlag, "--unset", "user.email"], successExitCodes: 5);
			}
			else
			{
				await ExecuteCommandAsync(repoPath, ["config", scopeFlag, "user.email", email]);
			}
		}

		private async Task EnsureCleanWorkingTreeAsync(string repoPath, [CallerMemberName] string caller = "")
		{
			var status = await GetStatusAsync(repoPath);
			if (status.Any())
			{
				throw new Exception($"Uncommitted changes are present [{caller}]");
			}
		}

		private async Task<int> GetRevListCountAsync(string repoPath, string range)
		{
			string rawOutput = await ExecuteCommandAsync(repoPath, ["rev-list", "--count", range], 128);
			return int.TryParse(rawOutput, out int count) ? count : 0;
		}
	}
}
