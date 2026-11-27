using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using System.Text.Json;

namespace ParekGIT.Data.Stores
{
	public class JsonStore : ISettingsStore, IThemeStore, IDisposable
	{
		private readonly IFileSystemService _fileSystem;
		private readonly ILogger _logger;

		private readonly string _settingsDir;
		private readonly string _settingsPath;
		private readonly string _themesDir;

		private readonly SemaphoreSlim _fileAccessLock = new(1, 1);
		private readonly JsonSerializerOptions _options = new() 
		{ 
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true, 
			PropertyNameCaseInsensitive = true 
		};

		// Constructor
		public JsonStore(IFileSystemService fileSystem, ILogger logger, IAppDataPaths appDataPaths)
		{
			_fileSystem = fileSystem;
			_logger = logger;

			_settingsDir = appDataPaths.GetSettingsDirectory();
			_settingsPath = Path.Combine(_settingsDir, "settings.json");
			_themesDir = appDataPaths.GetThemesDirectory();
		}

		public void Dispose() => _fileAccessLock.Dispose();

		// Settings
		public async Task EnsureSettingsFileAsync()
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				if (_fileSystem.FileExists(_settingsPath))
					return;

				await WriteUserSettingsInternalAsync(new UserSettings());
			}
			finally
			{
				_fileAccessLock.Release();
			}
		}

		public async Task<UserSettings> GetUserSettingsAsync()
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				return await ReadUserSettingsInternalAsync();
			}
			finally
			{
				_fileAccessLock.Release();
			}
		}

		public async Task SaveUserSettingsAsync(UserSettings settings)
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				await WriteUserSettingsInternalAsync(settings);
			}
			finally
			{
				_fileAccessLock.Release();
			}
		}

		public async Task<UserSettings> UpdateUserSettingsAsync(Func<UserSettings, bool> update)
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				UserSettings settings = await ReadUserSettingsInternalAsync();
				if (update(settings))
					await WriteUserSettingsInternalAsync(settings);

				return settings;
			}
			finally
			{
				_fileAccessLock.Release();
			}
		}

		// Themes
		public async Task EnsureDefaultThemesAsync()
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				if (!_fileSystem.DirectoryExists(_themesDir))
					await _fileSystem.CreateDirectory(_themesDir);

				var assembly = typeof(JsonStore).Assembly;
				const string prefix = "DefaultThemes/";

				foreach (string resourceName in assembly.GetManifestResourceNames().Where(n => n.StartsWith(prefix, StringComparison.Ordinal)))
				{
					string destination = Path.Combine(_themesDir, resourceName[prefix.Length..]);
					if (_fileSystem.FileExists(destination))
						continue;

					using var stream = assembly.GetManifestResourceStream(resourceName)!;
					using var reader = new StreamReader(stream);
					string content = await reader.ReadToEndAsync();
					await _fileSystem.WriteAllTextAsync(destination, content);
				}
			}
			finally
			{
				_fileAccessLock.Release();
			}
		}

		public async Task<IReadOnlyList<ThemeInfo>> LoadThemesAsync()
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				var themes = new List<ThemeInfo>();
				if (!_fileSystem.DirectoryExists(_themesDir))
					return themes;

				var files = await _fileSystem.EnumerateFilesAsync(_themesDir, "*.json");
				foreach (string file in files.OrderBy(f => f))
				{
					string id = Path.GetFileNameWithoutExtension(file).ToLower();
					if (!IsValidThemeToken(id)) continue;

					try
					{
						string json = await _fileSystem.ReadAllTextAsync(file);
						var theme = JsonSerializer.Deserialize<ThemeInfo>(json, _options);

						if (theme?.Colors == null) continue;
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
			finally
			{
				_fileAccessLock.Release();
			}
		}

		// Private helpers
		private async Task<UserSettings> ReadUserSettingsInternalAsync()
		{
			if (!_fileSystem.FileExists(_settingsPath))
				return new UserSettings();

			try
			{
				string json = await _fileSystem.ReadAllTextAsync(_settingsPath);
				return JsonSerializer.Deserialize<UserSettings>(json, _options) ?? new UserSettings();
			}
			catch (Exception ex) when (ex is JsonException or IOException)
			{
				await _logger.LogWarningAsync($"Failed to read '{_settingsPath}', falling back to defaults: {ex.Message}");
				return new UserSettings();
			}
		}

		private async Task WriteUserSettingsInternalAsync(UserSettings settings)
		{
			if (!_fileSystem.DirectoryExists(_settingsDir))
				await _fileSystem.CreateDirectory(_settingsDir);
			string json = JsonSerializer.Serialize<UserSettings>(settings, _options);
			await _fileSystem.WriteAllTextAtomicAsync(_settingsPath, json);
		}

		private static bool IsValidThemeToken(string token) =>
			!string.IsNullOrEmpty(token) && token.All(c => char.IsAsciiLetterOrDigit(c) || c == '-');

		private static bool IsSafeThemeValue(string? value) =>
			!string.IsNullOrWhiteSpace(value) && value.Length <= 100 && value.IndexOfAny([';', '{', '}', '<', '>', '\\' ]) < 0;
	}
}
