using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitWorkingTreeService : IGitWorkingTreeService
	{
		private readonly GitCommandExecutor _executor;
		private readonly IFileSystemService _fileSystem;

		// Constructor
		public GitWorkingTreeService(GitCommandExecutor executor, IFileSystemService fileSystem)
		{
			_executor = executor;
			_fileSystem = fileSystem;
		}

		// Public methods
		public async Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files)
		{
			if (files == null || !files.Any())
				throw new Exception("Cannot commit without changes [CommitAsync]");

			// Unstage everything
			await _executor.ExecuteAsync(repoPath, ["reset"]);

			// Stage selected files
			await _executor.ExecuteAsync(repoPath, ["add", "--", .. files]);

			// Prepare commit command
			List<string> commitArgs = ["commit", "-m", message];

			// Add description if typed in
			if (!string.IsNullOrWhiteSpace(desc))
				commitArgs.AddRange(["-m", desc]);

			await _executor.ExecuteAsync(repoPath, commitArgs);
		}

		public async Task DiscardChangeAsync(string repoPath, string filePath)
		{
			string[] statusArgs = ["status", "--porcelain", "--", filePath];
			string statusOutput = await _executor.ExecuteAsync(repoPath, statusArgs);
			if (string.IsNullOrWhiteSpace(statusOutput)) return;

			string statusCode = statusOutput.Length >= 2 ? statusOutput[..2] : "";
			if (statusCode == "??")
			{
				await _executor.ExecuteAsync(repoPath, ["clean", "-f", "--", filePath]);
			}
			else if (statusCode is "A " or "AM" or "AD")
			{
				// Staged new file
				await _executor.ExecuteAsync(repoPath, ["restore", "--staged", "--", filePath]);
				await _executor.ExecuteAsync(repoPath, ["clean", "-f", "--", filePath]);
			}
			else
			{
				// Modified/Deleted tracked file
				await _executor.ExecuteAsync(repoPath, ["restore", "--staged", "--worktree", "--", filePath]);
			}
		}

		public async Task DiscardAllChangesAsync(string repoPath)
		{
			await _executor.ExecuteAsync(repoPath, ["reset", "--hard", "HEAD"]);
			await _executor.ExecuteAsync(repoPath, ["clean", "-fd"]);
		}

		public async Task IgnoreFileAsync(string repoPath, string filePath)
		{
			string pattern = filePath.Replace("\\", "/");
			await AppendToGitignoreAsync(repoPath, pattern);
			await _executor.ExecuteAsync(repoPath, ["rm", "--cached", "--ignore-unmatch", "--", filePath]);
		}

		public async Task IgnoreFolderAsync(string repoPath, string filePath)
		{
			string pattern = filePath.Replace("\\", "/").TrimEnd('/') + "/";
			await AppendToGitignoreAsync(repoPath, pattern);
			await _executor.ExecuteAsync(repoPath, ["rm", "--cached", "-r", "--ignore-unmatch", "--", filePath]);
		}

		public async Task IgnoreExtensionAsync(string repoPath, string filePath)
		{
			string ext = Path.GetExtension(filePath);
			if (string.IsNullOrEmpty(ext))
				throw new ArgumentException($"File '{filePath}' has no extension to ignore.");
			string pattern = "*" + ext;
			await AppendToGitignoreAsync(repoPath, pattern);
			await _executor.ExecuteAsync(repoPath, ["rm", "--cached", "-r", "--ignore-unmatch", "--", $"*{ext}"]);
		}

		// Private helpers
		private async Task AppendToGitignoreAsync(string repoPath, string pattern)
		{
			string gitignorePath = Path.Combine(repoPath, ".gitingore");

			// Check if pattern not already included in .gitignore
			if (_fileSystem.FileExists(gitignorePath))
			{
				var lines = await _fileSystem.ReadAllLinesAsync(gitignorePath);
				if (lines.Any(line => line.Trim() == pattern)) return;
			}

			// Append pattern to the .gitignore file
			string contentToAppend = _fileSystem.FileExists(gitignorePath) && await _fileSystem.GetFileSizeAsync(gitignorePath) > 0
				? Environment.NewLine + pattern
				: pattern;
			await _fileSystem.AppendAllTextAsync(gitignorePath, contentToAppend + Environment.NewLine);
		}
	}
}
