using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using System.Text.Json;

namespace ParekGIT.Data.Data
{
	public class JsonStore : ISettingsStore
	{
		private readonly IFileSystemService _fileSystem;

		private readonly string _settingsDir;
		private readonly string _settingsPath;

		private readonly JsonSerializerOptions _options = new() 
		{ 
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true, 
			PropertyNameCaseInsensitive = true 
		};

		// Constructor
		public JsonStore(IFileSystemService fileSystem)
		{
			_fileSystem = fileSystem;

			_settingsDir = AppDataPaths.GetSettingsDirectory();
			_settingsPath = Path.Combine(_settingsDir, "settings.json");
		}

		// Settings
		public async Task EnsureSettingsFileAsync()
		{
			if (_fileSystem.FileExists(_settingsPath)) { return; }

			await SaveUserSettingsAsync(new UserSettings());
		}

		public async Task<UserSettings> GetUserSettingsAsync()
		{
			if (!_fileSystem.FileExists(_settingsPath))
				return new UserSettings();

			string json = await _fileSystem.ReadAllTextAsync(_settingsPath);
			var result = JsonSerializer.Deserialize<UserSettings>(json, _options) ?? new UserSettings();

			return result;
		}

		public async Task SaveUserSettingsAsync(UserSettings settings)
		{
			if (_settingsDir != null && !_fileSystem.DirectoryExists(_settingsDir))
				await _fileSystem.CreateDirectory(_settingsDir);

			string json = JsonSerializer.Serialize<UserSettings>(settings, _options);
			await _fileSystem.WriteAllTextAsync(_settingsPath, json);
		}
	}
}
