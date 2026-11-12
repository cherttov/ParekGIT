namespace ParekGIT.Core.Interfaces
{
	public interface IGitWorkingTreeService
	{
		Task CommitAsync(string repoPath, string message, string desc, IEnumerable<string> files);
		Task DiscardChangeAsync(string repoPath, string filePath);
		Task DiscardAllChangesAsync(string repoPath);
		Task IgnoreFileAsync(string repoPath, string filePath);
		Task IgnoreFolderAsync(string repoPath, string filePath);
		Task IgnoreExtensionAsync(string repoPath, string filePath);
	}
}
