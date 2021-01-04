using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSTrading.Shared.Domain
{
	public interface IRepository<T> where T : IAggregateRoot
	{
		Task AddAsync(T item);
		Task<T> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetAll();
		void Delete(T item);
		IUnitOfWork UnitOfWork { get; }
	}
}
