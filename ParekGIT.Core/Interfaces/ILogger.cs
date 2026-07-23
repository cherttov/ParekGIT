namespace ParekGIT.Core.Interfaces
{
	public interface ILogger
	{
		Task LogErrorAsync(string message, Exception? exception = null);
		Task LogWarningAsync(string message);
		Task LogInfoAsync(string message);
	}
}
