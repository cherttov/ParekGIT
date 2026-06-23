using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Core.Interfaces
{
    public interface IFileSystemService
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);

        Task<string> ReadAllTextAsync(string path);
        Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption);
    }
}
