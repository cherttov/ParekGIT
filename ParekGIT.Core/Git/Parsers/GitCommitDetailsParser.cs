using ParekGIT.Core.Git.Helpers;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitCommitDetailsParser
	{
		internal static CommitDetailsResult Parse(string rawOutput)
		{
			var lines = GitParserHelper.SplitLines(rawOutput);
			if (lines.Length == 0) 
				return new CommitDetailsResult { Author = "Unknown", Message = string.Empty, Files = [] };

			var headerParts = lines[0].Split('\x1f', 2);
			string author = headerParts[0].Trim();
			string message = headerParts.Length > 1 ? headerParts[1].Trim() : string.Empty;

			var files = new List<GitFileStatus>();
			foreach (var line in lines.Skip(1))
			{
				var parts = line.Split('\t');
				if (parts.Length < 2) continue;

				files.Add(new GitFileStatus
				{
					StatusCode = parts[0].Trim(),
					Path = parts[^1].Trim()
				});
			}

			return new CommitDetailsResult
			{
				Author = author,
				Message = message,
				Files = files
			};
		}
	}
}
