using System.Text.Json.Serialization;

namespace ParekGIT.Data.Models
{
	public class TodoItem
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid RepoId { get; set; }
		[JsonPropertyName("text")]
		public string TaskLabel { get; set; } = string.Empty;
		[JsonPropertyName("isCompleted")]
		public bool IsCompleted { get; set; } = false;
	}
}
