using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitConfigParser
	{
		internal static GitConfigInfo Parse(string rawOutput)
		{
			if (string.IsNullOrWhiteSpace(rawOutput)) { return new GitConfigInfo(); }

			string name = string.Empty;
			string email = string.Empty;

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var parts = line.Split(' ', 2);
				if (parts.Length < 2) { continue; }

				if (parts[0] == "user.name") { name = parts[1].Trim(); }
				else if (parts[0] == "user.email") { email = parts[1].Trim(); }
			}

			return new GitConfigInfo { Name = name, Email = email };
		}
	}
}
