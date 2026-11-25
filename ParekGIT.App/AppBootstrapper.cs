using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;
using ParekGIT.Data.Interfaces;
using Photino.NET;
using System.Drawing;

namespace ParekGIT.App
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
#if DEBUG
				.SetDevToolsEnabled(true)
#else
				.SetDevToolsEnabled(false)
#endif
				.SetIconFile("wwwroot/assets/icons/icon.ico")
				.SetJavascriptClipboardAccessEnabled(true)
				.Load("wwwroot/index.html");
		}

		public static void InitializeAppData(IServiceProvider serviceProvider)
		{
			var fileSystem = serviceProvider.GetRequiredService<IFileSystemService>();
			var appDataPaths = serviceProvider.GetRequiredService<IAppDataPaths>();
			foreach (string dir in appDataPaths.GetAllDirectories())
				fileSystem.CreateDirectory(dir).GetAwaiter().GetResult();

			var settingsStore = serviceProvider.GetRequiredService<ISettingsStore>();
			settingsStore.EnsureSettingsFileAsync().GetAwaiter().GetResult();

			var themeStore = serviceProvider.GetRequiredService<IThemeStore>();
			themeStore.EnsureDefaultThemesAsync().GetAwaiter().GetResult();
		}

		public static void HandleStartupError(Exception ex)
		{
			const string errorMessage = "Error occurred while starting ParekGIT";

			Console.WriteLine($"{errorMessage}:");
			if (OperatingSystem.IsLinux())
				Console.WriteLine("- WebKitGTK might be missing.");
			Console.WriteLine($"- Message: {ex.Message}");

			// Try logging startup error
			StartupLogger.LogFatal(errorMessage, ex);
		}
	}
}
