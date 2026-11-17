using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitStatusParser
	{
		internal static IEnumerable<GitFileStatus> Parse(string rawOutput)
		{
			var changedFiles = new List<GitFileStatus>();
			if (string.IsNullOrWhiteSpace(rawOutput)) return changedFiles;

			var tokens = rawOutput.Split('\0', StringSplitOptions.RemoveEmptyEntries);
			bool skipNext = false;
			foreach (var token in tokens)
			{
				if (skipNext) { skipNext = false; continue; }
				if (token.Length < 4) continue;

				string code = token[..2];
				skipNext = code[0] is 'R' or 'C' || code[1] is 'R' or 'C';

				changedFiles.Add(new GitFileStatus 
				{ 
					StatusCode = code, 
					Path = token[3..] 
				});
			}

			return changedFiles;
		}
	}
}
