namespace ParekGIT.Core.Interfaces
{
	public interface IFileSystemService
	{
		// Exist
		bool FileExists(string path);
		bool DirectoryExists(string path);

		// Create
		Task CreateDirectory(string path);

		// Write/read
		Task<string> ReadAllTextAsync(string path);
		Task WriteAllTextAsync(string path, string content);

		// Getters
		Task<IEnumerable<string>> GetFileSystemEntriesAsync(string path, string searchPattern, SearchOption searchOption);

		// Delete/recycle-bin
		Task MoveDirectoryToRecycleBinAsync(string path);
		Task DeleteFile(string path);
	}
}
