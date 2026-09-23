using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitHistoryParser
	{
		internal static IEnumerable<GitCommit> Parse(string rawOutput)
		{
			var commits = new List<GitCommit>();

			if (string.IsNullOrWhiteSpace(rawOutput)) { return commits; }

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var parts = line.Split('|');
				if (parts.Length != 4) { continue; }

				commits.Add(new GitCommit
				{
					Hash = parts[0],
					Message = parts[1],
					Author = parts[2],
					TimeAgo = parts[3]
				});
			}

			return commits;
		}
	}
}
