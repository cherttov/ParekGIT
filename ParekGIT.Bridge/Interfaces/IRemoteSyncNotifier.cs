namespace ParekGIT.Bridge.Interfaces
{
	public interface IRemoteSyncNotifier
	{
		void NotifyCommitsBehind(string repoPath, int commitsBehind);
	}
}
