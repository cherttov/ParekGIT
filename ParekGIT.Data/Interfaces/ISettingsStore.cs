using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface ISettingsStore
	{
		Task EnsureSettingsFileAsync();
		Task<UserSettings> GetUserSettingsAsync();
		Task SaveUserSettingsAsync(UserSettings settings);
		Task<UserSettings> UpdateUserSettingsAsync(Func<UserSettings, bool> update);
	}
}
