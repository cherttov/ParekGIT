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

			// Indexing repositories in DB
			var repos = _db.GetCollection<GitRepository>(ReposCollectionName);
			repos.EnsureIndex(r => r.LastAccessed);
			if (PathHelper.IsCaseSensitive)
				repos.EnsureIndex(r => r.AbsolutePath);
			else
				repos.EnsureIndex(r => r.AbsolutePath.ToLower());

			// Indexing todos in DB
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
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				return (IEnumerable<GitRepository>)repoCollection
					.Query()
					.OrderByDescending(entry => entry.LastAccessed)
					.ToList();
			});
		}

		public Task<GitRepository?> GetRepositoryAsync(string absolutePath)
		{
			return Task.Run(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				return FindRepositoryByPath(repoCollection, absolutePath);
			});
		}

		public Task UpsertRepositoryAsync(GitRepository repository)
		{
			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				repoCollection.Upsert(repository);
			});
		}

		public Task DeleteRepositoryAsync(string absolutePath)
		{
			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, absolutePath)
					?? throw new InvalidOperationException($"Repository not found for path: {absolutePath}");

				RunInTransaction(() =>
				{
					var todoCollection = _db.GetCollection<TodoItem>(TodosCollectionName);
					todoCollection.DeleteMany(todo => todo.RepoId == repo.Id);
					repoCollection.Delete(repo.Id);
				});
			});
		}

		public Task UpdateRepositoryValidStateAsync(string absolutePath, bool isValid)
		{
			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, absolutePath);
				if (repo != null && repo.IsValid != isValid)
				{
					repo.IsValid = isValid;
					repoCollection.Update(repo);
				}
			});
		}

		// Todos db
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

		public Task SaveBranchTodosAsync(string repoPath, string branchName, IEnumerable<TodoItem> todos)
		{
			var todosList = todos.ToList();

			
			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, repoPath)
					?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

				foreach (var todo in todosList)
				{
					todo.RepoId = repo.Id;
					todo.BranchName = branchName;
				}

				RunInTransaction(() =>
				{
					var todoCollection = _db.GetCollection<TodoItem>(TodosCollectionName);
					todoCollection.DeleteMany(todo => todo.RepoId == repo.Id && todo.BranchName == branchName);
					if (todosList.Count > 0)
						todoCollection.InsertBulk(todosList);
				});
			});
		}

		// Private helpers
		private async Task ExecuteWriteAsync(Action action)
		{
			await _writeLock.WaitAsync();
			try
			{
				await Task.Run(action);
			}
			finally
			{
				_writeLock.Release();
			}
		}

		private void RunInTransaction(Action action)
		{
			_db.BeginTrans();
			try
			{
				action();
				_db.Commit();
			}
			catch
			{
				_db.Rollback();
				throw;
			}
		}

		private static GitRepository? FindRepositoryByPath(ILiteCollection<GitRepository> collection, string absolutePath)
		{
			return PathHelper.IsCaseSensitive
				? collection.FindOne(r => r.AbsolutePath == absolutePath)
				: collection.FindOne(r => r.AbsolutePath.ToLower() == absolutePath.ToLower());
		}
	}
}
