using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Drawing;

namespace ParekGIT.UI
{
	internal static class AppBootstrapper
	{
		public static PhotinoWindow CreateWindow()
		{
			return new PhotinoWindow()
						.SetTitle("ParekGIT")
						.SetUseOsDefaultSize(false)
						.SetSize(new Size(960, 660))
						.SetMinSize(960, 660)
						.Center()
						.SetContextMenuEnabled(false)
						.SetDevToolsEnabled(true) // later false
						.SetIconFile("wwwroot/assets/icons/icon.ico")
						.SetJavascriptClipboardAccessEnabled(true)
						.Load("wwwroot/index.html");
		}

		public static void InitializeAppData(IServiceProvider serviceProvider)
		{
			var fileSystem = serviceProvider.GetRequiredService<IFileSystemService>();
			foreach (string dir in AppDataPaths.GetAllDirectories())
				fileSystem.CreateDirectory(dir).GetAwaiter().GetResult();

			var settingsStore = serviceProvider.GetRequiredService<ISettingsStore>();
			settingsStore.EnsureSettingsFileAsync().GetAwaiter().GetResult();

			var themeStore = serviceProvider.GetRequiredService<IThemeStore>();
			themeStore.EnsureDefaultThemesAsync().GetAwaiter().GetResult();
		}

		public static void HandleStartupError(Exception ex)
		{
			string errorMessage = "Error occurred while starting ParekGIT";

			if (OperatingSystem.IsLinux())
			{
				Console.WriteLine("Error occured while starting ParekGIT:");
				Console.WriteLine("- WebKitGTK might be missing.");
				Console.WriteLine($"- Message: {ex.Message}");
			}
			else
			{
				Console.WriteLine("Error occured while starting ParekGIT:");
				Console.WriteLine($"- Message: {ex.Message}");
			}

			// Try logging startup error
			try
			{
				var logger = new FileLogger();
				logger.LogErrorAsync(errorMessage, ex).GetAwaiter().GetResult();
			}
			catch 
			{
				// Failed to log on during startup crash
			}
		}
	}
}
