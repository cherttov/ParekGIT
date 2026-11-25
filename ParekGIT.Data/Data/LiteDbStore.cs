using ParekGIT.Core.Models;
using ParekGIT.Data.Interfaces;
using LiteDB;
using ParekGIT.Data.Models;
using ParekGIT.Core.Services;
using ParekGIT.Core.Interfaces;

namespace ParekGIT.Data.Data
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

			_db = new LiteDatabase($"Filename={rawPath}");

			var repos = _db.GetCollection<GitRepository>(ReposCollectionName);
			repos.EnsureIndex(r => r.LastAccessed);
			repos.EnsureIndex(r => r.AbsolutePath);

			var todos = _db.GetCollection<TodoItem>(TodosCollectionName);
			todos.EnsureIndex(t => t.RepoId);
			todos.EnsureIndex(t => t.BranchName);
		}

		public void Dispose()
		{
			_writeLock.Dispose();
			_db.Dispose();
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

		public Task<GitRepository?> GetRepositoryByPathAsync(string absolutePath)
		{
			return Task.Run<GitRepository?>(() =>
			{
				var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
				return collection.FindOne(r => r.AbsolutePath.ToLower() == absolutePath.ToLower());
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

		public Task DeleteRepositoryAsync(Guid id)
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<GitRepository>(ReposCollectionName);
				collection.Delete(id);
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
					var repo = collection.FindOne(r => r.AbsolutePath.ToLower() == absolutePath.ToLower());
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

		public Task<IEnumerable<TodoItem>> GetBranchTodosAsync(Guid repoId, string branchName)
		{
			return Task.Run(() =>
			{
				var collection = _db.GetCollection<TodoItem>(TodosCollectionName);
				return (IEnumerable<TodoItem>)collection
					.Find(todo => todo.RepoId == repoId && todo.BranchName == branchName)
					.ToList();
			});
		}

		public async Task<IEnumerable<TodoItem>> GetBranchTodosByPathAsync(string repoPath, string branchName)
		{
			var targetRepo = await GetRepositoryByPathAsync(repoPath)
				?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

			return await GetBranchTodosAsync(targetRepo.Id, branchName);
		}

		public async Task SaveBranchTodosAsync(Guid repoId, string branchName, IEnumerable<TodoItem> todos)
		{
			var todosList = todos.Select(t =>
			{
				t.RepoId = repoId;
				t.BranchName = branchName;
				return t;
			}).ToList();

			await _writeLock.WaitAsync();
			try
			{
				await Task.Run(() =>
				{
					_db.BeginTrans();
					try
					{
						var collection = _db.GetCollection<TodoItem>(TodosCollectionName);
						collection.DeleteMany(todo => todo.RepoId == repoId && todo.BranchName == branchName);
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

		public async Task SaveBranchTodosByPathAsync(string repoPath, string branchName, IEnumerable<TodoItem> todos)
		{
			var targetRepo = await GetRepositoryByPathAsync(repoPath)
				?? throw new InvalidOperationException($"Repository not found for path: {repoPath}");

			await SaveBranchTodosAsync(targetRepo.Id, branchName, todos);
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
	}
}
