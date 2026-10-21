namespace ParekGIT.Data.Models
{
	public class ThemeInfo
	{
		public string Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public Dictionary<string, string> Colors { get; set; }
	}
}
