using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Services
{
	internal sealed class RemoteSyncNotifier : IRemoteSyncNotifier
	{
		private readonly PhotinoWindow _window;

		// Constructor
		public RemoteSyncNotifier(PhotinoWindow window)
		{
			_window = window;
		}

		public void NotifyCommitsBehind(string repoPath, int commitsBehind, int commitsAhead)
		{
			// Response
			var message = IpcMessage.Create("REPO_SYNC_STATUS_CHANGED", new { repoPath, commitsBehind, commitsAhead });
			_window.SendWebMessage(JsonSerializer.Serialize(message));
		}
	}
}
