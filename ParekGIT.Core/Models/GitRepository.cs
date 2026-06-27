namespace ParekGIT.Core.Models
{
    public class GitRepository
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string AbsolutePath { get; set; } = string.Empty;
        public DateTime LastAccessed { get; set; } = DateTime.Now;
        public bool IsValid { get; set; } = true;
    }
}
