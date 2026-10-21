using ParekGIT.Data.Models;

namespace ParekGIT.Data.Interfaces
{
	public interface IThemeStore
	{
		Task EnsureDefaultThemesAsync();
		Task<IReadOnlyList<ThemeInfo>> LoadThemes();
	}
}
