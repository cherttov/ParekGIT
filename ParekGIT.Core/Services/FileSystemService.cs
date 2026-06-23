using CliWrap;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;
using ParekGIT.Core.Interfaces;
using System.Runtime.InteropServices;

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

        public async Task MoveDirectoryToRecycleBinAsync(string path)
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
