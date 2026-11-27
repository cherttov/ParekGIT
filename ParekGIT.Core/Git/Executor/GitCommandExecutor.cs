using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Git.Executor
{
	internal sealed class GitCommandExecutor
	{
		private readonly IFileSystemService _fileSystem;

		// Constructor
		public GitCommandExecutor(IFileSystemService fileSystem)
		{
			_fileSystem = fileSystem;
		}

		// Public methods
		public Task<string> ExecuteAsync(string? repoPath, IEnumerable<string> args, params int[] successExitCodes)
			=> ExecuteAsync(repoPath, args, CancellationToken.None, successExitCodes);

		public async Task<string> ExecuteAsync(string? repoPath, IEnumerable<string> args,
			CancellationToken cancellationToken, params int[] successExitCodes)
		{
			string safeWorkingDir = string.IsNullOrWhiteSpace(repoPath)
					? Environment.CurrentDirectory
					: repoPath;

			if (!_fileSystem.DirectoryExists(safeWorkingDir))
				throw new DirectoryNotFoundException($"Git command failed: working directory does not exist: '{safeWorkingDir}'");

			var cmd = Cli.Wrap("git")
					.WithArguments(args)
					.WithWorkingDirectory(safeWorkingDir)
					.WithValidation(CommandResultValidation.None);

			BufferedCommandResult result;
			try
			{
				result = await cmd.ExecuteBufferedAsync(cancellationToken);
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				throw new InvalidOperationException($"Git command failed: {ex.Message}", ex);
			}

			bool isAcceptable = result.ExitCode == 0 || successExitCodes.Contains(result.ExitCode);
			if (!isAcceptable)
				throw new InvalidOperationException($"Git command failed: exit {result.ExitCode}. {result.StandardError.Trim()}");

			return result.StandardOutput.TrimEnd('\r', '\n');
		}
	}
}
