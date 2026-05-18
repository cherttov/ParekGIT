using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Data;
using Photino.NET;
using System.Text.Json;
using CliWrap;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;

namespace ParekGIT.Bridge.Handlers
{
    public class RepoRemoveHandler : IMessageHandler
    {
        PhotinoWindow _window;
        LiteDbStore _dbStore;
        IGitRunner _gitRunner;

        public string Action => "REPO_REMOVE";

        // Constructor
        public RepoRemoveHandler(PhotinoWindow window, LiteDbStore dbStore, IGitRunner gitRunner)
        {
            _window = window;
            _dbStore = dbStore;
            _gitRunner = gitRunner;
        }

        public async Task ExecuteAsync(JsonElement payload)
        {
            string repoPath = payload.GetProperty("repoPath").GetString()
                ?? throw new ArgumentNullException("repoPath");

            bool deleteLocal = payload.GetProperty("deleteLocal").GetBoolean();

            var allRepos = await _dbStore.GetAllRepositoriesAsync();
            var repoToDelete = allRepos.FirstOrDefault(repo =>
                string.Equals(repo.AbsolutePath, repoPath, StringComparison.OrdinalIgnoreCase));

            // Delete from database
            if (repoToDelete != null)
            {
                await _dbStore.DeleteRepositoryAsync(repoToDelete.Id);
            }

            // Delete from drive
            if (deleteLocal && Directory.Exists(repoPath))
            {
                try
                {
                    await MoveToRecycleBinAsync(repoPath);
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"Could not delete repo. Directory might be in use: {ioEx}");
                }
                catch (UnauthorizedAccessException unAuthEx)
                {
                    Console.WriteLine($"Access denied: {unAuthEx}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while deleting repo: {ex}");
                }
            }

            // Response
            var response = new IpcMessage
            {
                Action = "REPO_REMOVED",
                Payload = JsonSerializer.SerializeToElement(new { absolutePath = repoPath })
            };

            _window.SendWebMessage(JsonSerializer.Serialize(response));
        }

        // Helpers
        private async Task MoveToRecycleBinAsync(string path)
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
