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

            var idk = await dbStore.GetAllRepositoriesAsync();

            foreach (var l in idk)
            {
                Console.WriteLine(l.Id);
                Console.WriteLine(l.Name);
                Console.WriteLine(l.AbsolutePath);
                Console.WriteLine(l.LastAccessed);
                Console.WriteLine("-------------------");
            }
        }
    }
}
