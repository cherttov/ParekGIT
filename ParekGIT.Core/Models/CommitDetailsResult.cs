using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Core.Models
{
	public class CommitDetailsResult
	{
		public string Author { get; set; }
		public string Message { get; set; }
		public List<GitFileStatus> Files { get; set; }
	}
}
