using System;
using CQRSTrading.Shared.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace CQRSTrading.Shared.Infrastructure
{
	public class EFTransactionAdapter : ITransaction
	{
		public Guid Id { get; private set; }
		public IDbContextTransaction DbContextTransaction { get; private set; }

		public EFTransactionAdapter(IDbContextTransaction transaction)
		{
			DbContextTransaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
			Id = Guid.NewGuid();
		}

		public void Dispose()
		{
			DbContextTransaction.Dispose();
		}

		public void Commit()
		{
			DbContextTransaction.Commit();
		}

		public void Rollback()
		{
			DbContextTransaction.Rollback();
		}
	}
}