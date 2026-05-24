using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Core.Interfaces
{
    internal interface IGitParser<T>
    {
        IEnumerable<T> Parse(string rawOutput);
    }
}
