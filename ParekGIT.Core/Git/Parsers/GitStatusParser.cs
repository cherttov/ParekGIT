using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitStatusParser
	{
		internal static IEnumerable<GitFileStatus> Parse(string rawOutput)
		{
			var changedFiles = new List<GitFileStatus>();

			if (string.IsNullOrWhiteSpace(rawOutput)) { return changedFiles; }

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				if (line.Length < 4) { continue; }

				changedFiles.Add(new GitFileStatus
				{
					StatusCode = line.Substring(0, 2),
					Path = line.Substring(3).Trim()
				});
			}

			return changedFiles;
		}
	}
}
