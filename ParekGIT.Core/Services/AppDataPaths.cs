namespace ParekGIT.Core.Services
{
	public static class AppDataPaths
	{
		private const string AppName = "ParekGIT";

		public static string GetAppDataRoot()
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
