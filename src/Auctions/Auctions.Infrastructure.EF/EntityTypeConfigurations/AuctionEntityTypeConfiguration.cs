using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Shared.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CQRSTrading.Auctions.Infrastructure.EF.EntityTypeConfigurations
{
	public class AuctionEntityTypeConfiguration : EntityTypeConfiguration<Auction>
	{
		public override void Configure(EntityTypeBuilder<Auction> builder)
		{
			base.Configure(builder);

			builder.OwnsOne(a => a.Name, ba =>
			{
				ba.Property(an => an.Value)
					.HasColumnName("Name")
					.HasMaxLength(AuctionName.MAX_LENGTH)
					.IsRequired();
			});

			builder.Navigation(a => a.Name)
				.IsRequired();

			builder.OwnsOne(a => a.Description, ba =>
			{
				ba.Property(ad => ad.Value)
					.HasColumnName("Description")
					.HasMaxLength(AuctionDescription.MAX_LENGTH)
					.IsRequired();
			});

			builder.Navigation(a => a.Description)
				.IsRequired();

			builder.OwnsOne(a => a.Price, ba =>
			{
				ba.Property(ap => ap.Value)
					.HasColumnName("Price")
					.IsRequired();
			});

			builder.Navigation(b => b.Price)
				.IsRequired();

			builder.Property(a => a.Quantity)
				.IsRequired();
		}
	}
}
