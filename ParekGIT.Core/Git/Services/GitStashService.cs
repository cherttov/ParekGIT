using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitStashService : IGitStashService
	{
		private readonly GitCommandExecutor _executor;

		// Constructor
		public GitStashService(GitCommandExecutor executor)
		{
			_executor = executor;
		}

		// Public methods
		public async Task<IEnumerable<GitStash>> GetStashesAsync(string repoPath) =>
			GitStashParser.Parse(await _executor.ExecuteAsync(repoPath, ["stash", "list", "--format=%gd|%H|%s"]));

		public async Task StashSaveAsync(string repoPath, string message, IEnumerable<string> files)
		{
			List<string> args = ["stash", "push", "--include-untracked"];

			if (!string.IsNullOrWhiteSpace(message))
				args.AddRange(["-m", message]);

			if (files?.Any() == true)
				args.AddRange(["--", .. files]);

			await _executor.ExecuteAsync(repoPath, args);
		}

		public async Task StashPopAsync(string repoPath, string stashSelector) =>
			await _executor.ExecuteAsync(repoPath, ["stash", "pop", stashSelector]);

		public async Task StashDropAsync(string repoPath, string stashSelector) =>
			await _executor.ExecuteAsync(repoPath, ["stash", "drop", stashSelector]);
	}
}
