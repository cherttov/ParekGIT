using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitBranchParser
	{
		private const string LOCAL_REF_PREFIX = "refs/heads/";
		private const string REMOTE_REF_PREFIX = "refs/remotes/";

		internal static IEnumerable<GitBranch> Parse(string rawOutput)
		{
			var branches = new List<GitBranch>();
			if (string.IsNullOrWhiteSpace(rawOutput)) { return branches; }

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var parts = line.Split('|');
				if (parts.Length != 5) { continue; }

				string fullRefName = parts[0].Trim();
				string name = parts[1].Trim();
				bool isCurrent = parts[2].Trim() == "*";
				string remoteBranch = parts[3].Trim();
				string targetCommitHash = parts[4].Trim();

				bool isRemote = fullRefName.StartsWith(REMOTE_REF_PREFIX, StringComparison.Ordinal); ;
				bool isLocal = fullRefName.StartsWith(LOCAL_REF_PREFIX, StringComparison.Ordinal);

				if (!isRemote && !isLocal) { continue; } // not local & not remote (how?)
				if (isRemote && fullRefName.EndsWith("/HEAD", StringComparison.Ordinal)) { continue; } // skip e.g. "origin/HEAD"

				branches.Add(new GitBranch
				{
					Name = name,
					IsCurrent = isCurrent,
					IsRemote = isRemote,
					TargetCommitHash = targetCommitHash,
					RemoteBranch = remoteBranch
				});
			}

			// Clean up 'origin/example' etc.
			var localBranchNames = branches
				.Where(b => !b.IsRemote)
				.Select(b => b.Name)
				.ToHashSet();

			var cleanBranchList = new List<GitBranch>();

			foreach (var branch in branches)
			{
				if (branch.IsRemote)
				{
					int slashIndex = branch.Name.IndexOf('/');
					string shortName = slashIndex >= 0 ? branch.Name[(slashIndex + 1)..] : branch.Name;

					if (localBranchNames.Contains(shortName))
					{
						continue;
					}
				}

				cleanBranchList.Add(branch);
			}

			return cleanBranchList;
		}
	}
}
