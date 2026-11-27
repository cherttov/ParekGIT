namespace ParekGIT.Core.Git.Helpers
{
	internal static class GitParserHelper
	{
		internal static string[] SplitLines(string rawOutput) =>
			string.IsNullOrWhiteSpace(rawOutput)
				? []
				: rawOutput.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
	}
}
