using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface ISettingsStore
	{
		Task<UserSettings> GetUserSettingsAsync();
		Task SaveUserSettingsAsync(UserSettings settings);
	}
}
