using ParekGIT.Core.Models;
using System.Text.RegularExpressions;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitStashParser
	{
		private static readonly Regex _branchPattern = new(@"^(?:WIP on|ON) ([^:]+):", RegexOptions.Compiled);

		internal static IEnumerable<GitStash> Parse(string rawOutput)
		{
			var stashes = new List<GitStash>();
			if (string.IsNullOrWhiteSpace(rawOutput)) { return stashes; }

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var parts = line.Split('|', 3);
				if (parts.Length < 3) { continue; }

				string selector = parts[0].Trim();
				string commitHash = parts[1].Trim();
				string message = parts[2].Trim();

				var branchMatch = _branchPattern.Match(message);
				string branchName = branchMatch.Success ? branchMatch.Groups[1].Value : string.Empty;

				stashes.Add(new GitStash
				{
					Selector = selector,
					CommitHash = commitHash,
					Message = message,
					BranchName = branchName
				});
			}

			return stashes;
		}
	}
}
