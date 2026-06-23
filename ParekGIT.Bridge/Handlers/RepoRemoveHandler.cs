using CliWrap;
using Microsoft.VisualBasic.FileIO;
using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Bridge.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoRemoveHandler : IMessageHandler
    {
        private readonly PhotinoWindow _window;
        private readonly LiteDbStore _dbStore;
        private readonly IFileSystemService _fileSystem;

        public string Action => "REPO_REMOVE";

        // Constructor
        public RepoRemoveHandler(PhotinoWindow window, LiteDbStore dbStore, IFileSystemService fileSystem)
        {
            _window = window;
            _dbStore = dbStore;
            _fileSystem = fileSystem;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new IpcPayloadException("repoPath");

            bool deleteLocal = payload.GetProperty("deleteLocal").GetBoolean();

            IEnumerable<GitRepository> allRepos = await _dbStore.GetAllRepositoriesAsync();
            GitRepository? repoToDelete = allRepos.FirstOrDefault(repo =>
                string.Equals(repo.AbsolutePath, repoPath, StringComparison.OrdinalIgnoreCase));

            // Delete from database
            if (repoToDelete != null)
            {
                await _dbStore.DeleteRepositoryAsync(repoToDelete.Id);
            }

            // Delete from drive
            bool localDeleteFailed = false;
            string? localDeleteError = null;

            if (deleteLocal && _fileSystem.DirectoryExists(repoPath))
            {
                try
                {
                    await MoveToRecycleBinAsync(repoPath);
                }
                catch (IOException ioEx)
                {
                    localDeleteFailed = true;
                    localDeleteError = "Directory might be in use.";
                    Console.WriteLine($"Could not delete repo. Directory might be in use: {ioEx}");
                }
                catch (UnauthorizedAccessException unAuthEx)
                {
                    localDeleteFailed = true;
                    localDeleteError = "Access denied.";
                    Console.WriteLine($"Access denied: {unAuthEx}");
                }
                catch (Exception ex)
                {
                    localDeleteFailed = true;
                    localDeleteError = ex.Message;
                    Console.WriteLine($"Error while deleting repo: {ex}");
                }
            }

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_REMOVED",
                Payload = JsonSerializer.SerializeToElement(new 
                { 
                    absolutePath = repoPath,
                    localDeleteFailed,
                    localDeleteError
                })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }

        // Helpers
        private async Task MoveToRecycleBinAsync(string path) // Move to Core
        {
            var directory = new DirectoryInfo(path);

            // Strip Read-Only attributes
            directory.Attributes = FileAttributes.Normal;
            foreach (var info in directory.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            // Match OS and delete accordingly
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FileSystem.DeleteDirectory(
                    path,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                await Cli.Wrap("osascript")
                    .WithArguments(["-e", $"tell application \"Finder\" to delete POSIX file \"{path}\""])
                    .ExecuteAsync();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                try
                {
                    await Cli.Wrap("gio")
                        .WithArguments(["trash", path])
                        .ExecuteAsync();
                }
                catch
                {
                    Console.WriteLine("Trash command not found. Falling back to permanent delete");
                    directory.Delete(true);
                }
            }
        }
    }
}
