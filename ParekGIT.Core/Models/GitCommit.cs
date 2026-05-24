using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Core.Models
{
    public class GitCommit
    {
        public string Hash { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public string TimeAgo { get; set; }
    }
}
