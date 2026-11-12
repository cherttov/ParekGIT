using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitConfigService : IGitConfigService
	{
		private readonly GitCommandExecutor _executor;

		// Constructor
		public GitConfigService(GitCommandExecutor executor)
		{
			_executor = executor;
		}

		// Public methods
		public async Task<GitConfigInfo> GetGlobalConfigAsync(string? repoPath = null) =>
			await GetConfigInternalAsync(repoPath, "--global");
		public async Task<GitConfigInfo> GetLocalConfigAsync(string repoPath) =>
			await GetConfigInternalAsync(repoPath, "--local");

		public async Task SaveLocalConfigAsync(string repoPath, string name, string email) =>
			await SaveConfigInternalAsync(repoPath, "--local", name, email);

		public async Task SaveGlobalConfigAsync(string? repoPath, string name, string email) =>
			await SaveConfigInternalAsync(repoPath, "--global", name, email);

		// Private helpers
		private async Task<GitConfigInfo> GetConfigInternalAsync(string? repoPath, string scopeFlag) =>
			GitConfigParser.Parse(await _executor.ExecuteAsync(repoPath, ["config", scopeFlag, "--get-regexp", @"^user\.(name|email)$"], 1));

		private async Task SaveConfigInternalAsync(string? repoPath, string scopeFlag, string name, string email)
		{
			// Name
			if (string.IsNullOrWhiteSpace(name))
				await _executor.ExecuteAsync(repoPath, ["config", scopeFlag, "--unset", "user.name"], successExitCodes: 5);
			else
				await _executor.ExecuteAsync(repoPath, ["config", scopeFlag, "user.name", name]);

			// Email
			if (string.IsNullOrWhiteSpace(email))
				await _executor.ExecuteAsync(repoPath, ["config", scopeFlag, "--unset", "user.email"], successExitCodes: 5);
			else
				await _executor.ExecuteAsync(repoPath, ["config", scopeFlag, "user.email", email]);
		}
	}
}
