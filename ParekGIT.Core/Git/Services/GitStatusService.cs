using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitStatusService : IGitStatusService
	{
		private readonly GitCommandExecutor _executor;
		private readonly IFileSystemService _fileSystem;

		// Constructor
		public GitStatusService(GitCommandExecutor executor, IFileSystemService fileSystem)
		{
			_executor = executor;
			_fileSystem = fileSystem;
		}

		// Public methods
		public async Task<IEnumerable<GitFileStatus>> GetStatusAsync(string repoPath) =>
			GitStatusParser.Parse(await _executor.ExecuteAsync(repoPath, ["status", "--porcelain", "-uall"]));

		public async Task<string> GetFileDiffAsync(string repoPath, string filePath)
		{
			string rawOutput = await _executor.ExecuteAsync(repoPath, ["diff", "HEAD", "--", filePath]);

			// If empty, must be untracked file (read directly)
			if (string.IsNullOrWhiteSpace(rawOutput))
			{
				string fullPath = Path.Combine(repoPath, filePath);
				if (_fileSystem.FileExists(fullPath))
				{
					// Add binary check here (use numstat)
					string rawText = await _fileSystem.ReadAllTextAsync(fullPath);
					var lines = rawText.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
					return string.Join("\n", lines.Select(line => "+" + line));
				}
			}

			return rawOutput;
		}

		public async Task<string> GetHistoryFileDiffAsync(string repoPath, string commitHash, string filePath) =>
			await _executor.ExecuteAsync(repoPath, ["show", "--diff-merges=first-parent", "--format=", commitHash, "--", filePath]);

		public async Task<int> GetCommitsAheadAsync(string repoPath) =>
			await GetRevListCountAsync(repoPath, "@{u}..HEAD");

		public async Task<int> GetCommitsBehindAsync(string repoPath) =>
			await GetRevListCountAsync(repoPath, "HEAD..@{u}");

		public async Task<string?> GetRemoteUrlAsync(string repoPath)
		{
			string rawOutput = await _executor.ExecuteAsync(repoPath, ["config", "--get", "remote.origin.url"], 1);
			return string.IsNullOrWhiteSpace(rawOutput) ? null : rawOutput;
		}

		// Private helpers
		private async Task<int> GetRevListCountAsync(string repoPath, string range)
		{
			string rawOutput = await _executor.ExecuteAsync(repoPath, ["rev-list", "--count", range], 128);
			return int.TryParse(rawOutput, out int count) ? count : 0;
		}
	}
}
