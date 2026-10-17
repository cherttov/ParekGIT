namespace ParekGIT.Data.Models
{
	public class UserSettings
	{
		// Visible parameters
		public string Theme { get; set; } = "catppuccin-mocha";

		// Hidden parameters
		public string LastRepoPath { get; set; } = string.Empty;
	}
}
