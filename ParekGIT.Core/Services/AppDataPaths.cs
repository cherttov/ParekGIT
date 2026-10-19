namespace ParekGIT.Core.Services
{
	public static class AppDataPaths
	{
		private const string AppName = "ParekGIT";

		private static readonly string _configDir; // themes/, settings.json
		private static readonly string _dataDir; // data.db
		private static readonly string _stateDir; // logs/

		// Constructor
		static AppDataPaths()
		{
			if (OperatingSystem.IsWindows())
			{
				// Windows
				string root = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
					AppName
				);
				_configDir = _dataDir = _stateDir = root;
			}
			else if (OperatingSystem.IsMacOS())
			{
				// MacOS
				string root = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
					"Library", "Application Support", AppName
				);
				_configDir = _dataDir = _stateDir = root;
			}
			else
			{
				// Linux
				_configDir = Path.Combine(XdgDir("XDG_CONFIG_HOME", ".config"), AppName);
				_dataDir = Path.Combine(XdgDir("XDG_DATA_HOME", Path.Combine(".local", "share")), AppName);
				_stateDir = Path.Combine(XdgDir("XDG_STATE_HOME", Path.Combine(".local", "state")), AppName);
			}
		}

		public static string GetConfigDirectory() { return _configDir; }
		public static string GetDatabaseDirectory() { return _dataDir; }
		public static string GetSettingsDirectory() { return _configDir; }
		public static string GetLogDirectory() { return Path.Combine(_stateDir, "logs"); }
		public static IEnumerable<string> GetAllDirectories() { return new[] { _configDir, _dataDir, GetLogDirectory() }.Distinct(); }

		// Private helpers
		private static string XdgDir(string envVar, string fallbackRelativeToHome)
		{
			string? value = Environment.GetEnvironmentVariable(envVar);
			if (!string.IsNullOrWhiteSpace(value) && Path.IsPathRooted(value)) { return value; }

			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				fallbackRelativeToHome
			);
		}
	}
}
