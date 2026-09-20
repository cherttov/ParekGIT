using ParekGIT.Core.Interfaces;
using Timer = System.Timers.Timer;

namespace ParekGIT.Core.Services
{
	public class RepoWatcher : IRepoWatcher
	{
		private ILogger _logger;
		private FileSystemWatcher? _watcher;
		private readonly Timer _debounceTimer;
		private string _currentRepoPath = string.Empty;

		public event EventHandler<string>? OnFilesChanged;

		// Constructor
		public RepoWatcher(ILogger logger)
		{
			_logger = logger;
			_debounceTimer = new Timer(500);
			_debounceTimer.AutoReset = false;
			_debounceTimer.Elapsed += (sender, e) => TimerElapsed();
		}

		public void WatchRepository(string repoPath)
		{
			_debounceTimer.Stop();

			_currentRepoPath = repoPath;

			// Dispose of existing watcher
			if (_watcher != null)
			{
				_watcher.EnableRaisingEvents = false;
				_watcher.Dispose();
			}

			_watcher = new FileSystemWatcher(repoPath)
			{
				IncludeSubdirectories = true,
				NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
			};

			// Event handlers
			_watcher.Changed += OnFileActivity;
			_watcher.Created += OnFileActivity;
			_watcher.Deleted += OnFileActivity;
			_watcher.Renamed += OnFileActivity;
			_watcher.Error += OnWatcherError;

			_watcher.EnableRaisingEvents = true;
		}

		private void OnFileActivity(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath.Contains($"{Path.DirectorySeparatorChar}.git")) { return; }

			_debounceTimer.Stop();
			_debounceTimer.Start();
		}

		private void OnWatcherError(object sender, ErrorEventArgs e)
		{
			_logger.LogWarningAsync($"FileSystemWatcher error on '{_currentRepoPath}' (likely buffer overflow)");

			if (!string.IsNullOrEmpty(_currentRepoPath))
			{
				WatchRepository(_currentRepoPath);
			}
		}

		private void TimerElapsed()
		{
			OnFilesChanged?.Invoke(this, _currentRepoPath);
		}

		public void Dispose()
		{
			if (_watcher != null)
			{
				_watcher.EnableRaisingEvents = false;
				_watcher.Dispose();
			}

			_debounceTimer?.Dispose();
		}
	}
}
