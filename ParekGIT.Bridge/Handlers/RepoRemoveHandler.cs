using ParekGIT.Bridge.Models;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Text.Json;
using ParekGIT.Core.Helpers;

namespace ParekGIT.Bridge.Handlers
{
	internal sealed class RepoRemoveHandler : IMessageHandler
	{
		private readonly PhotinoWindow _window;
		private readonly IRepositoryStore _repoStore;
		private readonly IFileSystemService _fileSystem;
		private readonly ILogger _logger;

		public string Action => "REPO_REMOVE";

		// Constructor
		public RepoRemoveHandler(PhotinoWindow window, IRepositoryStore repoStore, IFileSystemService fileSystem, ILogger logger)
		{
			_window = window;
			_repoStore = repoStore;
			_fileSystem = fileSystem;
			_logger = logger;
		}

		public async Task ExecuteAsync(JsonElement payload)
		{
			string repoPath = payload.GetProperty("repoPath").GetString()
				?? throw new IpcPayloadException("repoPath");

			bool deleteLocal = payload.GetProperty("deleteLocal").GetBoolean();

			IEnumerable<GitRepository> allRepos = await _repoStore.GetAllRepositoriesAsync();
			GitRepository? repoToDelete = allRepos.FirstOrDefault(repo => PathHelper.AreEqual(repo.AbsolutePath, repoPath));

			// Delete from database
			if (repoToDelete != null)
				await _repoStore.DeleteRepositoryAsync(repoToDelete.Id);

			// Delete from drive
			bool localDeleteFailed = false;
			string? localDeleteError = null;

			if (deleteLocal && _fileSystem.DirectoryExists(repoPath))
			{
				try
				{
					await _fileSystem.MoveDirectoryToRecycleBinAsync(repoPath);
				}
				catch (IOException ioEx)
				{
					localDeleteFailed = true;
					localDeleteError = "Directory might be in use.";
					await _logger.LogErrorAsync($"Could not delete repo at '{repoPath}'. Directory might be in use.", ioEx);
				}
				catch (UnauthorizedAccessException unAuthEx)
				{
					localDeleteFailed = true;
					localDeleteError = "Access denied.";
					await _logger.LogErrorAsync($"Access denied while deleting repo at '{repoPath}'.", unAuthEx);
				}
				catch (Exception ex)
				{
					localDeleteFailed = true;
					localDeleteError = ex.Message;
					await _logger.LogErrorAsync($"Error while deleting repo at '{repoPath}'.", ex);
				}
			}

			// Response
			var response = IpcMessage.Create("REPO_REMOVED", new
			{
				repoPath,
				localDeleteFailed,
				localDeleteError
			});
			_window.SendWebMessage(JsonSerializer.Serialize(response));
		}
	}
}
