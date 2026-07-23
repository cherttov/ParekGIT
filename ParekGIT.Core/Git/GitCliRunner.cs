using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Core.Services;

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

		// Command executor
		public async Task<string> ExecuteCommandAsync(string? repoPath, string arguments, params int[] successExitCodes)
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
				var result = await Cli.Wrap("git")
					.WithArguments(arguments)
					.WithWorkingDirectory(safeWorkingDir)
					.WithValidation(CommandResultValidation.None)
					.ExecuteBufferedAsync();

				bool isAcceptable = result.ExitCode == 0 || successExitCodes.Contains(result.ExitCode);

				if (!isAcceptable)
				{
					throw new Exception($"Git command failed: exit {result.ExitCode}. {result.StandardError.Trim()}");
				}

				// Just extra log
				if (result.ExitCode != 0)
				{
					await _logger.LogWarningAsync(
						$"git {arguments} exited {result.ExitCode} (acceptable). {result.StandardError.Trim()}"
					);
				}

				return result.StandardOutput.TrimEnd();
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				if (ex.Message.StartsWith("Git command failed")) { throw; }
				throw new Exception($"Git command failed: {ex.Message}", ex);
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

		public async Task<IEnumerable<GitCommit>> GetBranchHistoryAsync(string repoPath, string branchName, int skip = 0, int take = 50)
		{
			string arguments = $"log \"{branchName}\" -n {take} --pretty=format:\"%H|%s|%an|%ar\"";

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
					// Add binary check here
					string rawText = await _fileSystem.ReadAllTextAsync(fullPath);
					var lines = rawText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
					return string.Join("\n", lines.Select(line => "+" + line));
				}
			}

			return rawOutput;
		}

		public async Task<CommitDetailsResult> GetCommitDetailsAsync(string repoPath, string hash)
		{
			string arguments = $"show --name-status --format=\"%an%n%s\" {hash}";

			string rawOutput = await ExecuteCommandAsync(repoPath, arguments);

			return GitCommitDetailsParser.Parse(rawOutput);
		}

		public async Task<int> GetCommitsBehindAsync(string repoPath)
		{
			string arguments = "rev-list --count HEAD..@{u}";

			string output = await ExecuteCommandAsync(repoPath, arguments, 128);

			return int.TryParse(output, out int behindCount) ? behindCount : 0;
		}

		public async Task<string> GetHistoryFileDiffAsync(string repoPath, string commitHash, string filePath)
		{
			string arguments = $"show --format= {commitHash} -- \"{filePath}\"";

			return await ExecuteCommandAsync(repoPath, arguments);
		}

		public async Task<GitConfigInfo> GetGlobalConfigAsync(string? repoPath = null)
		{
			return new GitConfigInfo
			{
				Name = await ExecuteCommandAsync(repoPath, "config --global user.name", 1),
				Email = await ExecuteCommandAsync(repoPath, "config --global user.email", 1)
			};
		}

		public async Task<GitConfigInfo> GetLocalConfigAsync(string repoPath)
		{
			return new GitConfigInfo
			{
				Name = await ExecuteCommandAsync(repoPath, "config --local user.name", 1),
				Email = await ExecuteCommandAsync(repoPath, "config --local user.email", 1)
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

		public async Task CreateBranchFromCommitAsync(string repoPath, string branchName, string commitHash)
		{
			string arguments = $"checkout -b \"{branchName}\" {commitHash}";

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

		public async Task PushAsync(string repoPath)
		{
			string arguments = "push -u origin HEAD";

			await ExecuteCommandAsync(repoPath, arguments);
		}

		public async Task PullAsync(string repoPath)
		{
			string arguments = "pull";

			await ExecuteCommandAsync(repoPath, arguments);
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
					await _logger.LogWarningAsync(
						$"Revert conflict on commit '{commitHash}' in '{repoPath}'. Aborting. Git message: {ex.Message}"
					);

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
					await _logger.LogWarningAsync(
						$"Merge conflict while merging '{sourceBranch}' -> '{targetBranch}'. Aborting. Git message: {ex.Message}"
					);

					await ExecuteCommandAsync(repoPath, "merge --abort");

					throw new Exception($"Merge conflict occured while merging '{sourceBranch}' -> '{targetBranch}'. Merge aborted.");
				}

				throw;
			}
		}

		public async Task<GitRepository> CreateRepositoryAsync(CreateRepoRequest request)
		{
			// Create & check directory
			string fullPath = Path.Combine(request.LocalPath, request.RepoName);

			if (Directory.Exists(fullPath)) { throw new Exception($"Directory {request.RepoName} already exists."); }

			// Init repo
			Directory.CreateDirectory(fullPath);
			await ExecuteCommandAsync(fullPath, "init -b main");

			// gitignore if not None
			if (!string.Equals(request.GitIgnore, "None", StringComparison.OrdinalIgnoreCase))
			{
				await _templateService.WriteGitIgnoreAsync(fullPath, request.GitIgnore);
			}

			// license if not None
			if (!string.Equals(request.License, "None", StringComparison.OrdinalIgnoreCase))
			{
				GitConfigInfo globalConfig = await GetGlobalConfigAsync(null);

				await _templateService.WriteLicenseAsync(
					fullPath,
					request.License,
					string.IsNullOrWhiteSpace(request.LicenseYear) ? DateTime.Now.Year.ToString() : request.LicenseYear,
					string.IsNullOrWhiteSpace(request.LicenseOrganization) ? globalConfig.Name : request.LicenseOrganization,
					string.IsNullOrWhiteSpace(request.LicenseProject) ? request.RepoName : request.LicenseProject
				);
			}

			await ExecuteCommandAsync(fullPath, "add .");
			await ExecuteCommandAsync(fullPath, "commit --allow-empty -m \"Initial commit\"");

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
			string arguments = $"clone \"{repoUrl}\" \"{localPath}\"";

			await ExecuteCommandAsync(null, arguments);

			if (asLocal)
			{
				await ExecuteCommandAsync(localPath, "remote remove origin");
			}
		}

		public async Task SaveLocalConfigAsync(string repoPath, string name, string email)
		{
			// Handle Name
			if (string.IsNullOrWhiteSpace(name))
			{
				await ExecuteCommandAsync(repoPath, "config --local --unset user.name", successExitCodes: 5);
			}
			else
			{
				string safeName = name.Replace("\"", "\\\"");
				await ExecuteCommandAsync(repoPath, $"config --local user.name \"{safeName}\"");
			}

			// Handle Email
			if (string.IsNullOrWhiteSpace(email))
			{
				await ExecuteCommandAsync(repoPath, "config --local --unset user.email", successExitCodes: 5);
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
				await ExecuteCommandAsync(repoPath, "config --global --unset user.name", successExitCodes: 5);
			}
			else
			{
				string safeName = name.Replace("\"", "\\\"");
				await ExecuteCommandAsync(repoPath, $"config --global user.name \"{safeName}\"");
			}

			// Handle Email
			if (string.IsNullOrWhiteSpace(email))
			{
				await ExecuteCommandAsync(repoPath, "config --global --unset user.email", successExitCodes: 5);
			}
			else
			{
				string safeEmail = email.Replace("\"", "\\\"");
				await ExecuteCommandAsync(repoPath, $"config --global user.email \"{safeEmail}\"");
			}
		}
	}
}
