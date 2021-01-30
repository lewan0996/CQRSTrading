using System;
using System.Threading.Tasks;

namespace CQRSTrading.Shared.Domain
{
	public interface IRepository<T> where T : IAggregateRoot
	{
		Task AddAsync(T item);
		Task<T> GetByIdAsync(Guid id);
		void Delete(T item);
		IUnitOfWork UnitOfWork { get; }
	}
}
