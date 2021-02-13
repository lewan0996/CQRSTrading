using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSTrading.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace CQRSTrading.Shared.Infrastructure
{
	public abstract class WriteModelRepository<TEntity> : IWriteModelRepository<TEntity> where TEntity : Entity, IAggregateRoot
	{
		protected readonly DbContext DbContext;

		protected WriteModelRepository(DbContext dbContext, IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public IUnitOfWork UnitOfWork { get; }

		protected abstract IQueryable<TEntity> GetQueryWithAllIncludes();

		public virtual async Task<TEntity> GetByIdAsync(Guid id)
		{
			return await GetQueryWithAllIncludes()
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<IReadOnlyList<TEntity>> GetCollectionByIdsAsync(IEnumerable<Guid> ids)
		{
			return await
				DbContext.Set<TEntity>()
					.Where(e => ids.Any(id => id == e.Id))
					.ToListAsync();
		}

		public async Task AddAsync(TEntity entity)
		{
			await DbContext.Set<TEntity>()
				.AddAsync(entity);
		}

		public void Delete(TEntity entity)
		{
			DbContext.Set<TEntity>()
				.Remove(entity);
		}
	}
}
