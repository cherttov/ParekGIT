using System.Runtime.InteropServices;

namespace ParekGIT.Core.Services
{
	public static class AppDataPaths
	{
		private const string AppName = "ParekGIT";

		public static string GetAppDataRoot()
		{
			string basePath;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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

			return Path.Combine(basePath, AppName);
		}

		public static string GetLogDirectory()
		{
			return Path.Combine(GetAppDataRoot(), "logs");
		}

		public static string GetDatabaseDirectory()
		{
			return GetAppDataRoot();
		}
	}
}
