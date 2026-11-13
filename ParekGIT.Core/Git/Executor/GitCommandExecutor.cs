using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Git.Executor
{
	internal sealed class GitCommandExecutor
	{
		private readonly ILogger _logger;
		private readonly IFileSystemService _fileSystem;

		// Constructor
		public GitCommandExecutor(ILogger logger, IFileSystemService fileSystem)
		{
			_logger = logger;
			_fileSystem = fileSystem;
		}

		// Public methods
		public async Task<string> ExecuteAsync(string? repoPath, IEnumerable<string> args, params int[] successExitCodes)
			=> await ExecuteAsync(repoPath, args, CancellationToken.None, successExitCodes);

		public async Task<string> ExecuteAsync(string? repoPath, IEnumerable<string> args,
			CancellationToken cancellationToken = default, params int[] successExitCodes)
		{
			string safeWorkingDir = string.IsNullOrWhiteSpace(repoPath)
					? Environment.CurrentDirectory
					: repoPath;

			if (!_fileSystem.DirectoryExists(safeWorkingDir))
				throw new Exception($"Git command failed: working directory does not exist: '{safeWorkingDir}'");

			try
			{
				var cmd = Cli.Wrap("git")
					.WithArguments(args)
					.WithWorkingDirectory(safeWorkingDir)
					.WithValidation(CommandResultValidation.None);

				var result = await cmd.ExecuteBufferedAsync(cancellationToken);

				bool isAcceptable = result.ExitCode == 0 || successExitCodes.Contains(result.ExitCode);
				if (!isAcceptable)
					throw new Exception($"Git command failed: exit {result.ExitCode}. {result.StandardError.Trim()}");

				// Just an extra log
				if (result.ExitCode != 0)
					await _logger.LogWarningAsync($"{cmd} exited {result.ExitCode} (acceptable). {result.StandardError.Trim()}");

				return result.StandardOutput.TrimEnd();
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				if (ex.Message.StartsWith("Git command failed")) throw;
				throw new Exception($"Git command failed: {ex.Message}", ex);
			}
		}
	}
}
