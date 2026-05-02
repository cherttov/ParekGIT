using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Models;
using LiteDB;

namespace ParekGIT.Core.Data
{
    internal class LiteDbStore : IRepositoryStore
    {
        private readonly string _dbPath;
        private const string CollectionName = "repositories";

        // Constructor
        public LiteDbStore()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ParekGIT");

            Directory.CreateDirectory(appDataPath);

            _dbPath = Path.Combine(appDataPath, "data.db");
        }

        public Task<IEnumerable<GitRepository>> GetallRepositoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpsertRepositoryAsync(GitRepository repository)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRepositoryAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
