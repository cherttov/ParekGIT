using ParekGIT.Core.Interfaces;
using ParekGIT.Data.Interfaces;
using ParekGIT.Data.Models;
using System.Text.Json;

namespace ParekGIT.Data.Stores
{
	internal sealed class JsonStore : ISettingsStore, IThemeStore, IDisposable
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

		private bool _disposed;

		// Constructor
		public JsonStore(IFileSystemService fileSystem, ILogger logger, IAppDataPaths appDataPaths)
		{
			_fileSystem = fileSystem;
			_logger = logger;

			_settingsDir = appDataPaths.GetSettingsDirectory();
			_settingsPath = Path.Combine(_settingsDir, "settings.json");
			_themesDir = appDataPaths.GetThemesDirectory();
		}

		public void Dispose()
		{
			if (_disposed)
				return;
			_disposed = true;

			bool locked = _fileAccessLock.Wait(TimeSpan.FromSeconds(2));
			if (locked)
				_fileAccessLock.Release();
			_fileAccessLock.Dispose();
		}

		// Settings
		public Task EnsureSettingsFileAsync() => ExecuteLockedAsync(async () =>
		{
			if (_fileSystem.FileExists(_settingsPath))
				return;

			await WriteUserSettingsInternalAsync(new UserSettings());
		});

		public Task<UserSettings> GetUserSettingsAsync() =>
			ExecuteLockedAsync(ReadUserSettingsInternalAsync);

		public Task SaveUserSettingsAsync(UserSettings settings) =>
			ExecuteLockedAsync(() => WriteUserSettingsInternalAsync(settings));

		public Task<UserSettings> UpdateUserSettingsAsync(Func<UserSettings, bool> update) => ExecuteLockedAsync(async () =>
		{
			UserSettings settings = await ReadUserSettingsInternalAsync();
			if (update(settings))
				await WriteUserSettingsInternalAsync(settings);

			return settings;
		});

		// Themes
		public Task EnsureDefaultThemesAsync() => ExecuteLockedAsync(async () =>
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
				await _fileSystem.WriteAllTextAtomicAsync(destination, content);
			}
		});

		public Task<IReadOnlyList<ThemeInfo>> LoadThemesAsync() => ExecuteLockedAsync<IReadOnlyList<ThemeInfo>>(async () =>
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
					await _logger.LogWarningAsync($"Failed to load theme '{file}': {ex.Message}"); // Unreadable/malformed files -> log & skip
				}
			}

			return themes;
		});

		// Private helpers
		private Task ExecuteLockedAsync(Func<Task> action) =>
			ExecuteLockedAsync<object?>(async () =>
			{
				await action();
				return null;
			});

		private async Task<T> ExecuteLockedAsync<T>(Func<Task<T>> action)
		{
			await _fileAccessLock.WaitAsync();
			try
			{
				return await action();
			}
			finally
			{
				_fileAccessLock.Release();
			}
		}

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
