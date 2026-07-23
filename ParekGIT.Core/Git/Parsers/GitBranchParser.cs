using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitBranchParser
	{
		public static IEnumerable<GitBranch> Parse(string rawOutput)
		{
			var branches = new List<GitBranch>();

			// Empty repo
			if (string.IsNullOrWhiteSpace(rawOutput)) { return branches; }

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var parts = line.Split('|');
				if (parts.Length != 4) { continue; }

				string name = parts[0].Trim();
				if (name == "origin") { continue; }

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
					string shortName = branch.Name.Substring(branch.Name.IndexOf('/') + 1);

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
