using ParekGIT.Core.Data;
using ParekGIT.Core.Git;
using ParekGIT.Core.Models;
using System.Threading.Tasks;

namespace ParekGIT.Test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbStore = new LiteDbStore();
            var gitRunner = new GitCliRunner();

            var testRepo = new GitRepository
            {
                Name = "My Test Repository",
                AbsolutePath = @"C:\Users\Tymofii Chertov\Nevim\Programming\MyTestRepository",
                LastAccessed = DateTime.Now
            };

            await dbStore.UpsertRepositoryAsync(testRepo);
            try
            {
                var statusList = await gitRunner.GetStatusAsync(testRepo.AbsolutePath);
                if (statusList.Count == 0)
                {
                    Console.WriteLine("Status is empty");
                }
                else
                {
                    Console.WriteLine($"Found {statusList.Count} changed files");

                    foreach (var file in statusList)
                    {
                        Console.WriteLine($"[{file.StatusDescription}] ({file.StatusCode})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR" + ex);
            }
            Console.ReadLine();
        }
    }
}
