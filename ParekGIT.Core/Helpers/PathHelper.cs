namespace ParekGIT.Core.Helpers
{
	public static class PathHelper
	{
		public static bool IsCaseSensitive => OperatingSystem.IsLinux();

		public static StringComparison Comparison =>
			IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

		public static bool AreEqual(string? pathA, string? pathB) =>
			string.Equals(pathA, pathB, Comparison);
	}
}
