using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Services;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using System.Text.Json;

namespace ParekGIT.Data.Data
{
	public class JsonStore : ISettingsStore, IThemeStore
	{
		private readonly IFileSystemService _fileSystem;

		private readonly string _settingsDir;
		private readonly string _settingsPath;
		private readonly string _themesDir;

		private readonly SemaphoreSlim _settingsLock = new(1, 1);
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
			_themesDir = AppDataPaths.GetThemesDirectory();
		}

		// Settings
		public async Task EnsureSettingsFileAsync()
		{
			if (!_fileSystem.FileExists(_settingsPath))
				await SaveUserSettingsAsync(new UserSettings());
		}

		public async Task<UserSettings> GetUserSettingsAsync()
		{
			await _settingsLock.WaitAsync();
			try
			{
				if (!_fileSystem.FileExists(_settingsPath))
					return new UserSettings();
				string json = await _fileSystem.ReadAllTextAsync(_settingsPath);
				return JsonSerializer.Deserialize<UserSettings>(json, _options) ?? new UserSettings();
			}
			finally
			{
				_settingsLock.Release();
			}
		}

		public async Task SaveUserSettingsAsync(UserSettings settings)
		{
			await _settingsLock.WaitAsync();
			try
			{
				if (!_fileSystem.DirectoryExists(_settingsDir))
					await _fileSystem.CreateDirectory(_settingsDir);

				string json = JsonSerializer.Serialize<UserSettings>(settings, _options);
				await _fileSystem.WriteAllTextAsync(_settingsPath, json);
			}
			finally
			{
				_settingsLock.Release();
			}
		}

		// Themes
		public async Task EnsureDefaultThemesAsync()
		{
			if (!_fileSystem.DirectoryExists(_themesDir))
				await _fileSystem.CreateDirectory(_themesDir);

			var assembly = typeof(JsonStore).Assembly;
			const string prefix = "DefaultThemes/";

			foreach (string resourceName in assembly.GetManifestResourceNames().Where(n => n.StartsWith(prefix)))
			{
				string destination = Path.Combine(_themesDir, resourceName.Substring(prefix.Length));
				if (_fileSystem.FileExists(destination)) { continue; }

				using var stream = assembly.GetManifestResourceStream(resourceName)!;
				using var reader = new StreamReader(stream);
				await _fileSystem.WriteAllTextAsync(destination, await reader.ReadToEndAsync());
			}
		}

		public async Task<IReadOnlyList<ThemeInfo>> LoadThemes()
		{
			var themes = new List<ThemeInfo>();
			if (!_fileSystem.DirectoryExists(_themesDir))
				return themes;
				
			foreach (string file in Directory.EnumerateFiles(_themesDir, "*.json").OrderBy(f => f))
			{
				string id = Path.GetFileNameWithoutExtension(file).ToLower();
				if (!IsValidThemeToken(id)) { continue; }

				try
				{
					string json = await _fileSystem.ReadAllTextAsync(file);
					var theme = JsonSerializer.Deserialize<ThemeInfo>(json, _options);

					if (theme?.Colors == null) { continue; }
					theme.Id = id;

					if (string.IsNullOrWhiteSpace(theme.Name))
						theme.Name = id;

					var badKeys = theme.Colors
						.Where(kv => !IsValidThemeToken(kv.Key) || !IsSafeThemeValue(kv.Value))
						.Select(kv => kv.Key)
						.ToList();

					foreach (var key in badKeys)
						theme.Colors.Remove(key);

					if (theme.Colors.Count > 0)
						themes.Add(theme);
				}
				catch (Exception ex) when (ex is JsonException or IOException)
				{
					// Unreadable/malformed files -> skip
				}
			}

			return themes;
		}

		// Private helpers
		private static bool IsValidThemeToken(string token)
		{
			return !string.IsNullOrEmpty(token) && token.All(c => char.IsAsciiLetterOrDigit(c) || c == '-');
		}

		private static bool IsSafeThemeValue(string? value)
		{
			return !string.IsNullOrWhiteSpace(value) && value.Length <= 100 && value.IndexOfAny([';', '{', '}', '<', '>', '\\' ]) < 0;
		}
	}
}
