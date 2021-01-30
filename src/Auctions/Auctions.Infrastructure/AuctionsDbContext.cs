using CQRSTrading.Auctions.Domain.AuctionAggregate;
using Microsoft.EntityFrameworkCore;

namespace CQRSTrading.Auctions.Infrastructure
{
	public class AuctionsDbContext : DbContext
	{
		public DbSet<Auction> Auctions { get; set; }

		public AuctionsDbContext(DbContextOptions<AuctionsDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionsDbContext).Assembly);
			modelBuilder.HasDefaultSchema("Auctions");
		}
	}
}
