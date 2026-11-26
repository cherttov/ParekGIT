using Microsoft.Extensions.DependencyInjection;
using ParekGIT.Data.Stores;
using ParekGIT.Data.Interfaces;

namespace ParekGIT.Data
{
	public static class DataServiceExtensions
	{
		public static IServiceCollection AddDataServices(this IServiceCollection services)
		{
			services.AddSingleton<LiteDbStore>();
			services.AddSingleton<JsonStore>();
			services.AddSingleton<IRepositoryStore>(p => p.GetRequiredService<LiteDbStore>());
			services.AddSingleton<ITodoStore>(p => p.GetRequiredService<LiteDbStore>());
			services.AddSingleton<ISettingsStore>(p => p.GetRequiredService<JsonStore>());
			services.AddSingleton<IThemeStore>(p => p.GetRequiredService<JsonStore>());

			return services;
		}
	}
}
