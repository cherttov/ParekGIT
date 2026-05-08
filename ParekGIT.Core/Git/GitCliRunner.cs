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

        // Parsers
        public async Task<IEnumerable<GitBranch>> GetBranchesAsync(string repositoryPath)
        {
            string arguments = "branch --all --format=\"%(refname:short)|%(HEAD)|%(upstream:short)|%(objectname)\"";

            string rawOutput = await ExecuteCommandAsync(repositoryPath, arguments);

            return GitBranchParser.Parse(rawOutput);
        }

        // Commands
        public async Task CheckoutBranchAsync(string repositoryPath, string branchName, bool isRemote)
        {
            string arguments = isRemote
                ? $"checkout -t \"{branchName}\""
                : $"checkout \"{branchName}\"";

            await ExecuteCommandAsync(repositoryPath, arguments);
        }
    }
}
