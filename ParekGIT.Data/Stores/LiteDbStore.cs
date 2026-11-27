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
				repos.EnsureIndex(r => r.AbsolutePath, unique: true);
			else
				repos.EnsureIndex(r => r.AbsolutePath.ToLower(), unique: true);

			// Indexing todos in DB
			var todos = _db.GetCollection<TodoItem>(TodosCollectionName);
			todos.EnsureIndex(t => t.RepoId);
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
			string normalizedPath = PathHelper.Normalize(absolutePath);

			return Task.Run(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				return FindRepositoryByPath(repoCollection, normalizedPath);
			});
		}

		public Task UpsertRepositoryAsync(GitRepository repository)
		{
			repository.AbsolutePath = PathHelper.Normalize(repository.AbsolutePath);

			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var existing = FindRepositoryByPath(repoCollection, repository.AbsolutePath);
				if (existing != null)
					repository.Id = existing.Id;

				repoCollection.Upsert(repository);
			});
		}

		public Task DeleteRepositoryAsync(string absolutePath)
		{
			string normalizedPath = PathHelper.Normalize(absolutePath);

			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath)
					?? throw new InvalidOperationException($"Repository not found for path: {normalizedPath}");

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
			string normalizedPath = PathHelper.Normalize(absolutePath);

			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath);
				if (repo != null && repo.IsValid != isValid)
				{
					repo.IsValid = isValid;
					repoCollection.Update(repo);
				}
			});
		}

		public Task TouchRepositoryAsync(string absolutePath)
		{
			string normalizedPath = PathHelper.Normalize(absolutePath);

			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath);
				if (repo != null)
				{
					repo.LastAccessed = DateTime.UtcNow;
					repoCollection.Update(repo);
				}
			});
		}

		// Todos db
		public Task<IEnumerable<TodoItem>> GetBranchTodosAsync(string repoPath, string branchName)
		{
			string normalizedPath = PathHelper.Normalize(repoPath);

			return Task.Run(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath)
					?? throw new InvalidOperationException($"Repository not found for path: {normalizedPath}");

				var todoCollection = _db.GetCollection<TodoItem>(TodosCollectionName);
				return (IEnumerable<TodoItem>)todoCollection
					.Find(todo => todo.RepoId == repo.Id && todo.BranchName == branchName)
					.ToList();
			});
		}

		public Task SaveBranchTodosAsync(string repoPath, string branchName, IEnumerable<TodoItem> todos)
		{
			string normalizedPath = PathHelper.Normalize(repoPath);
			var todosList = todos.ToList();
			
			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath)
					?? throw new InvalidOperationException($"Repository not found for path: {normalizedPath}");

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

		public Task RenameBranchTodosAsync(string repoPath, string oldBranchName, string newBranchName)
		{
			string normalizedPath = PathHelper.Normalize(repoPath);

			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath)
					?? throw new InvalidOperationException($"Repository not found for path: {normalizedPath}");

				RunInTransaction(() =>
				{
					var todoCollection = _db.GetCollection<TodoItem>(TodosCollectionName);
					var branchTodos = todoCollection
						.Find(todo => todo.RepoId == repo.Id && todo.BranchName == oldBranchName)
						.ToList();

					if (branchTodos.Count == 0)
						return;

					foreach (var todo in branchTodos)
						todo.BranchName = newBranchName;

					todoCollection.Update(branchTodos);
				});
			});
		}

		public Task DeleteBranchTodosAsync(string repoPath, string branchName)
		{
			string normalizedPath = PathHelper.Normalize(repoPath);

			return ExecuteWriteAsync(() =>
			{
				var repoCollection = _db.GetCollection<GitRepository>(ReposCollectionName);
				var repo = FindRepositoryByPath(repoCollection, normalizedPath)
					?? throw new InvalidOperationException($"Repository not found for path: {normalizedPath}");

				var todoCollection = _db.GetCollection<TodoItem>(TodosCollectionName);
				todoCollection.DeleteMany(todo => todo.RepoId == repo.Id && todo.BranchName == branchName);
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
