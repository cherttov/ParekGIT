namespace ParekGIT.Data.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TaskLabel { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
    }
}
