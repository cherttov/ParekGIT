using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using LiteDB;
using ParekGIT.Data.Models;
using ParekGIT.Core.Interfaces;
using ParekGIT.Core.Helpers;

namespace ParekGIT.Data.Stores
{
	public class LiteDbStore : IRepositoryStore, ITodoStore, IDisposable
	{
		private readonly LiteDatabase _db;

		private const string ReposCollectionName = "repositories";
		private const string TodosCollectionName = "todos";

		private readonly SemaphoreSlim _writeLock = new(1, 1);

		// Constructor
		public LiteDbStore(IAppDataPaths appDataPaths)
		{
			string appDataPath = appDataPaths.GetDatabaseDirectory();
			string rawPath = Path.Combine(appDataPath, "data.db");

			_db = new LiteDatabase(new ConnectionString { Filename = rawPath });

			var repos = _db.GetCollection<GitRepository>(ReposCollectionName);
			repos.EnsureIndex(r => r.LastAccessed);
			repos.EnsureIndex(r => r.AbsolutePath);

			var todos = _db.GetCollection<TodoItem>(TodosCollectionName);
			todos.EnsureIndex(t => t.RepoId);
			todos.EnsureIndex(t => t.BranchName);
		}

		public void Dispose()
		{
			_db.Dispose();
			_writeLock.Dispose();
		}

		// Repos db
		public Task<IEnumerable<GitRepository>> GetAllRepositoriesAsync()
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
				return (IEnumerable<GitRepository>)collection
					.Query()
					.OrderByDescending(entry => entry.LastAccessed)
					.ToList();
			});
		}

		public Task<GitRepository?> GetRepositoryAsync(string absolutePath)
		{
			return Task.Run<GitRepository?>(() =>
			{
				var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
				return FindRepositoryByPath(collection, absolutePath);
			});
		}

		public Task UpsertRepositoryAsync(GitRepository repository)
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
				collection.Upsert(repository);
			});
		}

		public Task DeleteRepositoryAsync(string absolutePath)
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(collection, absolutePath)
					?? throw new InvalidOperationException($"Repository not found for path: {absolutePath}");

				collection.Delete(repo.Id);
			});
		}

		public async Task UpdateRepositoryValidStateAsync(string absolutePath, bool isValid)
		{
			await _writeLock.WaitAsync();
			try
			{
				await Task.Run(() =>
				{
					var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
					var repo = FindRepositoryByPath(collection, absolutePath);
					if (repo != null && repo.IsValid != isValid)
					{
						repo.IsValid = isValid;
						collection.Update(repo);
					}
				});
			}
			finally
			{
				_writeLock.Release();
			}
		}

		// Todos db
		public Task<IEnumerable<TodoItem>> GetAllTodosAsync()
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<TodoItem>(TodosCollectionName);
				return (IEnumerable<TodoItem>)collection
					.FindAll()
					.ToList();
			});
		}

		public Task<IEnumerable<TodoItem>> GetBranchTodosAsync(string repoPath, string branchName)
		{
			return Task.Run(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, repoPath)
					?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

				var todoCollection = _db.GetCollection<TodoItem>(TodosCollectionName);
				return (IEnumerable<TodoItem>)todoCollection
					.Find(todo => todo.RepoId == repo.Id && todo.BranchName == branchName)
					.ToList();
			});
		}

		public async Task SaveBranchTodosAsync(string repoPath, string branchName, IEnumerable<TodoItem> todos)
		{
			var todosList = todos.ToList();

			await _writeLock.WaitAsync();
			try
			{
				await Task.Run(() =>
				{
					var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
					var repo = FindRepositoryByPath(repoCollection, repoPath)
						?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

					foreach (var todo in todosList)
					{
						todo.RepoId = repo.Id;
						todo.BranchName = branchName;
					}

					_db.BeginTrans();
					try
					{
						var collection = _db.GetCollection<TodoItem>(TodosCollectionName);
						collection.DeleteMany(todo => todo.RepoId == repo.Id && todo.BranchName == branchName);
						if (todosList.Count > 0)
							collection.InsertBulk(todosList);
						_db.Commit();
					}
					catch
					{
						_db.Rollback();
						throw;
					}
				});
			}
			finally
			{
				_writeLock.Release();
			}
		}

		public Task UpsertTodoAsync(TodoItem todo)
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<TodoItem>(TodosCollectionName);
				collection.Upsert(todo);
			});
		}

		public Task DeleteTodoAsync(Guid id)
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<TodoItem>(TodosCollectionName);
				collection.Delete(id);
			});
		}

		// Private helpers
		private static GitRepository? FindRepositoryByPath(ILiteCollection<GitRepository> collection, string absolutePath)
		{
			return PathHelper.IsCaseSensitive
				? collection.FindOne(r => r.AbsolutePath == absolutePath)
				: collection.FindOne(r => r.AbsolutePath.ToLower() == absolutePath.ToLower());
		}
	}
}
