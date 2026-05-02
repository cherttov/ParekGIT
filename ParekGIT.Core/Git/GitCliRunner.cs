using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Git
{
    internal class GitCliRunner : IGitRunner
    {
        public async Task<string> ExecuteCommandAsync(string repositoryPath, string arguments)
        {
            try
            {
                var result = await Cli.Wrap("git")
                    .WithArguments(arguments)
                    .WithWorkingDirectory(repositoryPath)
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteBufferedAsync();

                if (result.ExitCode != 0)
                {
                    throw new Exception($"Git Error: {result.StandardError}");
                }

                return result.StandardOutput.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to run git command: {ex.Message}");
            }
        }
    }
}
