using ParekGIT.Bridge.Data;
using ParekGIT.Bridge.Interfaces;
using ParekGIT.Core.Interfaces;
using Photino.NET;
using System.Text.Json;

namespace ParekGIT.Bridge.Services
{
	public class RemoteSyncNotifier : IRemoteSyncNotifier
	{
		private readonly PhotinoWindow _window;
		private readonly IGitRunner _gitRunner;

		// Constructor
		public RemoteSyncNotifier(PhotinoWindow window, IGitRunner gitRunner)
		{
			_window = window;
			_gitRunner = gitRunner;
		}

		public void NotifyCommitsBehind(string repoPath, int commitsBehind)
		{
			// Response
			var message = new IpcMessage
			{
				Action = "REMOTE_SYNC_STATUS",
				Payload = JsonSerializer.SerializeToElement(new { repoPath, commitsBehind })
			};
			_window.SendWebMessage(JsonSerializer.Serialize(message));
		}
	}
}
