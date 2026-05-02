using CliWrap;
using CliWrap.Buffered;
using ParekGIT.Core.Git.Parsers;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git
{
    public class GitCliRunner : IGitRunner
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

        public async Task<List<GitFileStatus>> GetStatusAsync(string repositoryPath)
        {
            string rawOutput = await ExecuteCommandAsync(repositoryPath, "status --porcelain");

            return GitStatusParser.ParsePorcelain(rawOutput);
        }
    }
}
