using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Services
{
    public class FileSystemService : IFileSystemService
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public Task<string> ReadAllTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }
        public async Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption)
        {
            return await Task.Run(() =>
                Directory.GetFileSystemEntries(path, searchPattern, searchOption)
            );
        }
    }
}
