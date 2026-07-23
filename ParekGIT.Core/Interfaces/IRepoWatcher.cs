using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Core.Interfaces
{
	public interface IRepoWatcher : IDisposable
	{
		event EventHandler<string> OnFilesChanged;
		void WatchRepository(string repoPath);
	}
}
