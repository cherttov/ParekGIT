using ParekGIT.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Core.Services
{
    public class FileSystemService : IFileSystemService
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public Task<string> ReadAllTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }
    }
}
