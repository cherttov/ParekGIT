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
				string contentToAppend = _fileSystem.FileExists(gitignorePath) && await _fileSystem.GetFileSizeAsync(gitignorePath) > 0
					? Environment.NewLine + normalizedFilePath
					: normalizedFilePath;

				await _fileSystem.AppendAllTextAsync(gitignorePath, contentToAppend + Environment.NewLine);
			}

			await _executor.ExecuteAsync(repoPath, ["rm", "--cached", "--ignore-unmatch", "--", filePath]);
		}

		public Task IgnoreFolderAsync(string repoPath, string filePath)
		{
			throw new NotImplementedException();
		}

		public Task IgnoreExtensionAsync(string repoPath, string filePath)
		{
			throw new NotImplementedException();
		}
	}
}
