using System.Linq;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Infrastructure;

namespace CQRSTrading.Auctions.Infrastructure
{
	public class AuctionsWriteModelRepository : WriteModelRepository<Auction>
	{
		public AuctionsWriteModelRepository(AuctionsDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork) { }
		protected override IQueryable<Auction> GetQueryWithAllIncludes() => ((AuctionsDbContext)DbContext).Auctions;
	}
}
