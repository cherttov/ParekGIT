using ParekGIT.Core.Git.Executor;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Services
{
	internal sealed class GitRepoService : IGitRepoService
	{
		private readonly GitCommandExecutor _executor;
		private readonly ITemplateService _templateService;
		private readonly IFileSystemService _fileSystem;
		private readonly IGitConfigService _config;

		// Constructor
		public GitRepoService(GitCommandExecutor executor, ITemplateService templateService, IFileSystemService fileSystem, IGitConfigService config)
		{
			_executor = executor;
			_templateService = templateService;
			_fileSystem = fileSystem;
			_config = config;
		}

		// Public methods
		public async Task<GitRepository> CreateRepositoryAsync(CreateRepoRequest request)
		{
			// Create & check directory
			string fullPath = Path.Combine(request.LocalPath, request.RepoName);
			if (_fileSystem.DirectoryExists(fullPath))
				throw new Exception($"Directory {request.RepoName} already exists.");

			// Init repo
			await _fileSystem.CreateDirectory(fullPath);
			await _executor.ExecuteAsync(fullPath, ["init", "-b", "main"]);

			// gitignore if not None
			if (!string.Equals(request.GitIgnore, "None", StringComparison.OrdinalIgnoreCase))
				await _templateService.WriteGitIgnoreAsync(fullPath, request.GitIgnore);

			// license if not None
			if (!string.Equals(request.License, "None", StringComparison.OrdinalIgnoreCase))
			{
				GitConfigInfo globalConfig = await _config.GetGlobalConfigAsync(null);

				string year = string.IsNullOrWhiteSpace(request.LicenseYear) ? DateTime.Now.Year.ToString() : request.LicenseYear;
				string org = string.IsNullOrWhiteSpace(request.LicenseOrganization) ? globalConfig.Name : request.LicenseOrganization;
				string proj = string.IsNullOrWhiteSpace(request.LicenseProject) ? request.RepoName : request.LicenseProject;

				await _templateService.WriteLicenseAsync(fullPath, request.License, year, org, proj);
			}

			await _executor.ExecuteAsync(fullPath, ["add", "."]);
			await _executor.ExecuteAsync(fullPath, ["commit", "--allow-empty", "-m", "Initial commit"]);

			return new GitRepository
			{
				Id = Guid.NewGuid(),
				Name = request.RepoName,
				AbsolutePath = fullPath,
				LastAccessed = DateTime.UtcNow
			};
		}

		public async Task CloneRepositoryAsync(string repoUrl, bool asLocal, string localPath, CancellationToken cancellationToken = default)
		{
			await _executor.ExecuteAsync(null, ["clone", repoUrl, localPath], cancellationToken);

			if (asLocal)
				await _executor.ExecuteAsync(localPath, ["remote", "remove", "origin"]);
		}
	}
}
