using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using LiteDB;
using ParekGIT.Data.Models;

namespace ParekGIT.Data.Data
{
    public class LiteDbStore : IRepositoryStore, ISettingsStore
    {
        private readonly string _dbPath;
        private const string ReposCollectionName = "repositories";
        private const string SettingsCollectionName = "settings";

        // Constructor
        public LiteDbStore()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ParekGIT");

            Directory.CreateDirectory(appDataPath);

            _dbPath = Path.Combine(appDataPath, "data.db");
        }

        // Repos db
        public Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

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
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

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
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

                    collection.Delete(id);
                }
            });
        }

        // Settings db
        public Task<UserSettings> GetUserSettingsAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<UserSettings>(SettingsCollectionName);
                    var settings = collection.FindById(1);
                    return settings ?? new UserSettings();
                }
            });
        }

        public Task SaveUserSettingsAsync(UserSettings settings)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<UserSettings>(SettingsCollectionName);
                    collection.Upsert(settings);
                }
            });
        }
    }
}
