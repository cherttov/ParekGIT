namespace ParekGIT.Core.Interfaces
{
	public interface IFileSystemService
	{
		// Exist
		bool FileExists(string path);
		bool DirectoryExists(string path);

		// Create
		Task CreateDirectory(string path);

		// Read
		Task<string> ReadAllTextAsync(string path);
		Task<string[]> ReadAllLinesAsync(string path);

		// Write
		Task WriteAllTextAsync(string path, string content);
		Task AppendAllTextAsync(string path, string content);

		// Getters
		Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption);
		Task<long> GetFileSizeAsync(string path);

		// Delete/RecycleBin
		Task MoveDirectoryToRecycleBinAsync(string path);
		Task DeleteFile(string path);
	}
}
