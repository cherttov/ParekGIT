using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core;
using ParekGIT.Data;

namespace ParekGIT.UI
{
	internal class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				// Creating ServiceCollection
				var services = new ServiceCollection()
					.AddCoreServices()
					.AddDataServices()
					.AddBridgeServices()
					.AddSingleton(_ => AppBootstrapper.CreateWindow());

				// Building ServiceProvider
				using var serviceProvider = services.BuildServiceProvider();

				// Initializizing APP_DATA (directories/files)
				AppBootstrapper.InitializeAppData(serviceProvider);
				
				// Registering handlers to IpcRouter
				var window = serviceProvider.GetRequiredService<PhotinoWindow>();
				var router = serviceProvider.GetRequiredService<IIpcRouter>();
				_ = serviceProvider.GetRequiredService<IRemoteSyncNotifier>();

				foreach (var handler in serviceProvider.GetServices<IMessageHandler>())
					router.RegisterHandler(handler);

				// Routing frontend messages to IpcRouter
				window.RegisterWebMessageReceivedHandler(router.HandleMessage!);

				// Starting the app
				var app = new PhotinoApplication();
				app.Run(window);
			}
			catch (Exception ex)
			{
				AppBootstrapper.HandleStartupError(ex);
			}
		}
	}
}
