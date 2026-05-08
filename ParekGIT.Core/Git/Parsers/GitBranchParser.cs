using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
    internal class GitBranchParser
    {
        public static IEnumerable<GitBranch> Parse(string rawOutput)
        {
            var branches = new List<GitBranch>();

            var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Empty repo
            if (string.IsNullOrWhiteSpace(rawOutput)) return branches;

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length != 4) continue;

                string name = parts[0].Trim();
                if (name == "origin") continue;

                bool isCurrent = parts[1].Trim() == "*";
                bool isRemote = name.Contains('/') && !isCurrent;
                string targetCommitHash = parts[3].Trim();
                string remoteBranch = parts[2].Trim();

                branches.Add(new GitBranch
                {
                    Name = name,
                    IsCurrent = isCurrent,
                    IsRemote = isRemote,
                    TargetCommitHash = targetCommitHash,
                    RemoteBranch = remoteBranch
                });
            }

            return branches;
        }
    }
}
