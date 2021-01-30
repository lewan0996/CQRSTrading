using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CQRSTrading.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CQRSTrading.Shared.Infrastructure
{
	// ReSharper disable once InconsistentNaming
	public class EFUnitOfWork : IUnitOfWork
	{
		private readonly DbContext[] _contexts;
		public ITransaction CurrentTransaction { get; private set; }

		public EFUnitOfWork(params DbContext[] contexts)
		{
			if (contexts == null || contexts.Length == 0)
			{
				throw new ArgumentException(nameof(contexts));
			}

			_contexts = contexts;
		}

		public async Task<ITransaction> BeginTransactionAsync()
		{
			if (HasActiveTransaction)
			{
				return CurrentTransaction;
			}

			var newDbContextTransaction =
				await _contexts[0]
					.Database
					.BeginTransactionAsync(IsolationLevel.ReadCommitted);

			var newTransactionDecorator =
				new EFTransactionAdapter(newDbContextTransaction);

			CurrentTransaction = newTransactionDecorator;

			await UseTransactionInAllContextsAsync(newDbContextTransaction);

			return CurrentTransaction;
		}

		private async Task UseTransactionInAllContextsAsync(IDbContextTransaction dbContextTransaction)
		{
			foreach (var dbContext in _contexts)
			{
				if (dbContext.Database.CurrentTransaction == null)
				{
					await dbContext
						.Database.UseTransactionAsync(dbContextTransaction.GetDbTransaction());
				}
				else if (dbContext.Database.CurrentTransaction != dbContextTransaction)
				{
					throw new InvalidOperationException(
						$"DbContext {dbContext.ContextId} has already different transaction attached - Id: {dbContext.Database.CurrentTransaction.TransactionId}");
				}
			}
		}

		public async Task<int> SaveEntitiesAsync()
		{
			var saveChangesTasks = _contexts.Select(c => c.SaveChangesAsync());
			var rowsAffectedPerTask = await Task.WhenAll(saveChangesTasks);

			return rowsAffectedPerTask.Sum();
		}

		public async Task CommitTransactionAsync(ITransaction transaction)
		{
			if (transaction == null)
			{
				throw new ArgumentNullException(nameof(transaction));
			}

			if (transaction != CurrentTransaction)
			{
				throw new InvalidOperationException($"Transaction {transaction.Id} is not current");
			}

			try
			{
				await SaveEntitiesAsync();
				CurrentTransaction.Commit();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
			finally
			{
				if (CurrentTransaction != null)
				{
					CurrentTransaction.Dispose();
					CurrentTransaction = null;
				}

				await ClearTransactionFromAllContextsAsync();
			}
		}

		public void RollbackTransaction()
		{
			try
			{
				CurrentTransaction?.Rollback();
			}
			finally
			{
				if (CurrentTransaction != null)
				{
					CurrentTransaction.Dispose();
					CurrentTransaction = null;
				}
			}
		}

		public bool HasActiveTransaction => CurrentTransaction != null;

		private async Task ClearTransactionFromAllContextsAsync()
		{
			foreach (var dbContext in _contexts)
			{
				await dbContext.Database.UseTransactionAsync(null);
			}
		}
	}
}