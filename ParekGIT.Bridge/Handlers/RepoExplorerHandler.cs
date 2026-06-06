using ParekGIT.Bridge.Interfaces;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ParekGIT.Bridge.Handlers
{
    public class ExplorerOpenHandler : IMessageHandler
    {
        public string Action => "EXPLORER_OPEN";

        public Task ExecuteAsync(JsonElement payload)
        {
            string rawPath = payload.GetProperty("path").GetString()
                ?? throw new ArgumentNullException("path");

            if (string.IsNullOrEmpty(rawPath)) { return Task.CompletedTask; }

            string normalizedPath = Path.GetFullPath(rawPath);

            if (!Directory.Exists(normalizedPath)) { return Task.CompletedTask; }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", normalizedPath);
            } 
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = normalizedPath,
                    UseShellExecute = true
                });
            }

            return Task.CompletedTask;
        }
    }
}
