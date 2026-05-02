using ParekGIT.Core.Data;
using ParekGIT.Core.Models;
using System.Threading.Tasks;

namespace ParekGIT.Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbStore = new LiteDbStore();

            var myTestRepo = new GitRepository
            {
                Name = "My Test Repository",
                AbsolutePath = @"C:\Users\Tymofii Chertov\Nevim\Programming\MyTestRepository",
                LastAccessed = DateTime.Now
            };

            Console.WriteLine($"Saving...: {myTestRepo.Name}");

            await dbStore.UpsertRepositoryAsync(myTestRepo);
            Console.WriteLine("Saved");

            Console.WriteLine("Reading...");
            var allRepos = await dbStore.GetAllRepositoriesAsync();

            foreach (var repo in allRepos)
            {
                Console.WriteLine($"Found Repo: {repo.Name}");
                Console.WriteLine($"Path: {repo.AbsolutePath}");
                Console.WriteLine($"Id: {repo.Id}");
                Console.WriteLine($"Last Accessed: {repo.LastAccessed}");
            }

            Console.WriteLine("Completed");
            Console.ReadLine();
        }
    }
}
