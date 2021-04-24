using System;
using System.Threading.Tasks;

namespace CQRSTrading.Auctions.ReadModel
{
	public interface IAuctionsReadModelRepository
	{
		Task<Auction> GetAsync(Guid id, string category);
		Task<Auction> GetAsync(Guid id, Guid userId);
		Task Insert(Auction auction);
	}
}
