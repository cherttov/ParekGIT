using ParekGIT.Core.Git;
using ParekGIT.Core.Models;
using ParekGIT.Data.Data;

namespace ParekGIT.Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbStore = new LiteDbStore();
            var gitRunner = new GitCliRunner();
        }
    }
}
