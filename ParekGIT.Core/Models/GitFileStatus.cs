namespace ParekGIT.Core.Models
{
    public class GitFileStatus
    {
        public string FilePath { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        public string StatusDescription => GetDescription();

        private string GetDescription()
        {
            if (StatusCode == "??") return "Untracked";
            if (StatusCode.Contains("M")) return "Modified";
            if (StatusCode.Contains("A")) return "Added";
            if (StatusCode.Contains("D")) return "Deleted";
            return "Unknown";
        }
    }
}
