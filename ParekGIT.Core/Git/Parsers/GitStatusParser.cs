using ParekGIT.Core.Models;

namespace ParekGIT.Core.Git.Parsers
{
    internal static class GitStatusParser
    {
        public static List<GitFileStatus> ParsePorcelain(string rawOutput)
        {
            var result = new List<GitFileStatus>();

            var lines = rawOutput.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // each porcelain line at least 4 char
                if (line.Length < 4) continue; 

                // status code
                string code = line.Substring(0, 2); 

                // after 3rd char file path
                string path = line.Substring(3).Trim();

                result.Add(new GitFileStatus
                {
                    StatusCode = code,
                    FilePath = path
                });
            }

            return result;
        }
    }
}
