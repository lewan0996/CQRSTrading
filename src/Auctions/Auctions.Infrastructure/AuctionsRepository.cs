using System.Linq;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Infrastructure;

namespace CQRSTrading.Auctions.Infrastructure
{
	public class AuctionsRepository : Repository<Auction>
	{
		public AuctionsRepository(AuctionsDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork) { }
		protected override IQueryable<Auction> GetQueryWithAllIncludes() => ((AuctionsDbContext)DbContext).Auctions;
	}
}
