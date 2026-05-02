namespace ParekGIT.Core.Models
{
    internal class GitRepository
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string AbsolutePath { get; set; } = string.Empty;
        public DateTime LastAccessed { get; set; } = DateTime.Now;
    }
}
