using ParekGIT.Core.Interfaces;

namespace ParekGIT.Core.Services
{
	public class AppDataPaths : IAppDataPaths
	{
		private const string AppName = "ParekGIT";

		private readonly string _configDir; // themes/, settings.json
		private readonly string _dataDir; // data.db
		private readonly string _stateDir; // logs/

		// Constructor
		public AppDataPaths()
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

		public string GetConfigDirectory() => _configDir;
		public string GetDatabaseDirectory() => _dataDir;
		public string GetSettingsDirectory() => _configDir;
		public string GetLogDirectory() => Path.Combine(_stateDir, "logs");
		public string GetThemesDirectory() => Path.Combine(_configDir, "themes");
		public IEnumerable<string> GetAllDirectories() => new[] { _configDir, _dataDir, GetLogDirectory(), GetThemesDirectory() }.Distinct(); 

		// Private helpers
		private static string XdgDir(string envVar, string fallbackRelativeToHome)
		{
			string? value = Environment.GetEnvironmentVariable(envVar);
			if (!string.IsNullOrWhiteSpace(value) && Path.IsPathRooted(value)) return value;

			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				fallbackRelativeToHome
			);
		}
	}
}
