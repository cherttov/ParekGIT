using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
    internal class GitCommitDetailsParser
    {
        public static CommitDetailsResult Parse(string rawOutput)
        {
            var lines = rawOutput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
            {
                return new CommitDetailsResult { Author = "Unknown", Files = new List<GitFileStatus>() };
            }

            string author = lines[0].Trim();
            string message = lines.Length > 1 ? lines[1].Trim() : "";
            List<GitFileStatus> files = lines.Skip(2).Select(f => {
                var parts = f.Split('\t');
                return new GitFileStatus
                {
                    StatusCode = parts[0].Trim(),
                    Path = parts.Length > 1 ? parts[1].Trim() : parts[0].Trim()
                };
            }).ToList();

            return new CommitDetailsResult
            {
                Author = author,
                Message = message,
                Files = files
            };
        }
    }
}
