using ParekGIT.Core.Interfaces;
using Timer = System.Timers.Timer;

namespace ParekGIT.Core.Services
{
	public class RepoWatcher : IRepoWatcher
	{
		private FileSystemWatcher? _watcher;
		private readonly Timer _debounceTimer;
		private string? _currentRepoPath;

		public event EventHandler<string>? OnFilesChanged;

		// Constructor
		public RepoWatcher()
		{
			_debounceTimer = new Timer(500);
			_debounceTimer.AutoReset = false;
			_debounceTimer.Elapsed += (sender, e) => TimerElapsed();
		}

		public void WatchRepository(string repoPath)
		{
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

			_watcher.EnableRaisingEvents = true;
		}

		private void OnFileActivity(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath.Contains($"{Path.DirectorySeparatorChar}.git")) { return; }

			_debounceTimer.Stop();
			_debounceTimer.Start();
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
