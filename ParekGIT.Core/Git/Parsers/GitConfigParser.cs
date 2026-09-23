using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
	internal static class GitConfigParser
	{
		internal static GitConfigInfo Parse(string rawOutput)
		{
			var config = new GitConfigInfo { Name = string.Empty, Email = string.Empty };
			if (string.IsNullOrWhiteSpace(rawOutput)) { return config; }

			var lines = rawOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var parts = line.Split(' ', 2);
				if (parts.Length < 2) { continue; }

				if (parts[0] == "user.name") { config.Name = parts[1].Trim(); }
				else if (parts[0] == "user.email") { config.Email = parts[1].Trim(); }
			}

			return config;
		}
	}
}
