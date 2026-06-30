namespace ParekGIT.Core.Interfaces
{
    public interface IFileSystemService
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);

        Task<string> ReadAllTextAsync(string path);
        Task WriteAllTextAsync(string path, string content);

        Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption);

        Task MoveDirectoryToRecycleBinAsync(string path);
        Task DeleteFile(string path);
    }
}
