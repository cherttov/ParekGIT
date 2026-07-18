using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using LiteDB;
using ParekGIT.Data.Models;
using ParekGIT.Core.Services;

namespace ParekGIT.Data.Data
{
    public class LiteDbStore : IRepositoryStore, ISettingsStore, ITodoStore
    {
        private readonly string _connectionString;

        private const string ReposCollectionName = "repositories";
        private const string SettingsCollectionName = "settings";
        private const string TodosCollectionName = "todos";

        // Constructor
        public LiteDbStore()
        {
            string appDataPath = appDataPath = AppDataPaths.GetAppDataRoot();

            Directory.CreateDirectory(appDataPath);

            string rawPath = Path.Combine(appDataPath, "data.db");
            _connectionString = $"Filename={rawPath};Connection=Shared";
        }

        // Repos db
        public Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

                    return (IEnumerable<GitRepository>)collection
                        .Query()
                        .OrderByDescending(entry => entry.LastAccessed)
                        .ToList();
                }
            });
        }

        public Task<GitRepository?> GetRepositoryByPathAsync(string absolutePath)
        {
            return Task.Run<GitRepository?>(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

                    return (GitRepository)collection.FindOne(r => r.AbsolutePath.ToLower() == absolutePath.ToLower());
                }
            });
        }

        public Task UpsertRepositoryAsync(GitRepository repository)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
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
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

                    collection.Delete(id);
                }
            });
        }

        public Task UpdateRepositoryValidStateAsync(string absolutePath, bool isValid)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<GitRepository>(ReposCollectionName);

                    var repo = collection.FindOne(r => r.AbsolutePath.ToLower() == absolutePath.ToLower());

                    if (repo != null && repo.IsValid != isValid)
                    {
                        repo.IsValid = isValid;
                        collection.Update(repo);
                    }
                }
            });
        }

        // Settings db
        public Task<UserSettings> GetUserSettingsAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
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
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<UserSettings>(SettingsCollectionName);
                    collection.Upsert(settings);
                }
            });
        }

        // Todos db
        public Task<IEnumerable<TodoItem>> GetAllTodosAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<TodoItem>(TodosCollectionName);

                    return (IEnumerable<TodoItem>)collection
                        .FindAll()
                        .ToList();
                }
            });
        }

        public Task<IEnumerable<TodoItem>> GetRepoTodosAsync(Guid repoId)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<TodoItem>(TodosCollectionName);

                    return (IEnumerable<TodoItem>)collection
                        .Find(todo => todo.RepoId == repoId)
                        .ToList();
                }
            });
        }

        public Task SaveRepoTodosAsync(Guid repoId, IEnumerable<TodoItem> todos)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<TodoItem>(TodosCollectionName);

                    collection.DeleteMany(todo => todo.RepoId == repoId);

                    if (todos.Any()) { collection.InsertBulk(todos); }
                }
            });
        }

        public Task UpsertTodoAsync(TodoItem todo)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<TodoItem>(TodosCollectionName);

                    collection.Upsert(todo);
                }
            });
        }

        public Task DeleteTodoAsync(Guid id)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(_connectionString))
                {
                    var collection = db.GetCollection<TodoItem>(TodosCollectionName);

                    collection.Delete(id);
                }
            });
        }
    }
}
