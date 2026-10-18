using System.Net.NetworkInformation;

namespace ParekGIT.Core.Services
{
	public static class AppDataPaths
	{
		private const string AppName = "ParekGIT";

		private static readonly string _appDataRoot;

		// Constructor
		static AppDataPaths()
		{
			string basePath;

			if (OperatingSystem.IsWindows())
			{
				basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			}
			else if (OperatingSystem.IsMacOS())
			{
				basePath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
					"Library", "Application Support"
				);
			}
			else
			{
				basePath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
					".local", "share"
				);
			}

			_appDataRoot = Path.Combine(basePath, AppName);
		}

		public static string GetAppDataRoot() { return _appDataRoot; }

		public static string GetLogDirectory() { return Path.Combine(_appDataRoot, "logs"); }

		public static string GetDatabaseDirectory() { return _appDataRoot; }

		public static string GetSettingsDirectory() { return _appDataRoot; }
	}
}
