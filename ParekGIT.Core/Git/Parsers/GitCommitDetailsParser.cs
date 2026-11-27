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
				return new CommitDetailsResult { Author = "Unknown", Message = string.Empty, Files = new List<GitFileStatus>() };

			string author = lines[0].Trim();
			string message = lines.Length > 1 ? lines[1].Trim() : "";
			List<GitFileStatus> files = lines.Skip(2).Select(f =>
			{
				var parts = f.Split('\t');
				return new GitFileStatus
				{
					StatusCode = parts[0].Trim(),
					Path = parts[^1].Trim()
				};
			}).ToList();

			return new CommitDetailsResult
			{
				Author = author,
				Message = message,
				Files = files
			};
		}
	}
}
