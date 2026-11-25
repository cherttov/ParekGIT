namespace ParekGIT.Core.Services
{
	public static class StartupLogger
	{
		public static void LogFatal(string message, Exception ex)
		{
			try
			{
				var logger = new FileLogger(new AppDataPaths());
				logger.LogErrorAsync(message, ex).GetAwaiter().GetResult();
			}
			catch
			{
				// Failed to log during startup crash
			}
		}
	}
}
