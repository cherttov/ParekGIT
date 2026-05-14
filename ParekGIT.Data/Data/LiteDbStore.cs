using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using LiteDB;

namespace ParekGIT.Data.Data
{
    public class LiteDbStore : IRepositoryStore
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

        public Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<GitRepository>(CollectionName);

                    return (IEnumerable<GitRepository>)collection.Query()
                        .OrderByDescending(entry => entry.LastAccessed)
                        .ToList();
                }
            });
        }

        public Task UpsertRepositoryAsync(GitRepository repository)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<GitRepository>(CollectionName);

                    collection.Upsert(repository);
                }
            });
        }

        public Task DeleteRepositoryAsync(Guid id)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<GitRepository>(CollectionName);

                    collection.Delete(id);
                }
            });
        }
    }
}
