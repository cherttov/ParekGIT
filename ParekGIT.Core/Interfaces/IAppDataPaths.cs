namespace ParekGIT.Core.Interfaces
{
	public interface IAppDataPaths
	{
		string GetConfigDirectory();
		string GetDatabaseDirectory();
		string GetSettingsDirectory();
		string GetLogDirectory();
		string GetThemesDirectory();
		IEnumerable<string> GetAllDirectories();
	}
}
