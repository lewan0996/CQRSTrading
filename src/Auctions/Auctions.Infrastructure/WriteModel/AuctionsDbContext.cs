using CQRSTrading.Auctions.Domain.AuctionAggregate;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CQRSTrading.Auctions.Infrastructure.WriteModel
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
