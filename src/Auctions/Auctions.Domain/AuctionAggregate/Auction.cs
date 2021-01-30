using System;
using CQRSTrading.Shared.Domain;

namespace CQRSTrading.Auctions.Domain.AuctionAggregate
{
	public class Auction : Entity, IAggregateRoot
	{
		public Guid UserId { get; private set; }
		public AuctionName Name { get; private set; }
		public AuctionDescription Description { get; private set; }
		public Price Price { get; private set; }
		public int Quantity { get; private set; }
		public string ImageBase64 { get; private set; }

		public Auction(Guid id, Guid userId, AuctionName name, AuctionDescription description, Price price, int quantity, string imageBase64 = null) : base(id)
		{
			if (userId == default)
			{
				throw new DomainException("UserId must not have a default value");
			}

			UserId = userId;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			Price = price ?? throw new ArgumentNullException(nameof(price));

			if (quantity <= 0)
			{
				throw new DomainException($"{nameof(quantity)} must be greater than 0");
			}

			Quantity = quantity;

			if (imageBase64 != null)
			{
				AddImage(imageBase64);
			}
		}

		// ReSharper disable once UnusedMember.Local
		private Auction() { } // For EF

		public void AddImage(string imageBase64)
		{
			if (string.IsNullOrWhiteSpace(imageBase64))
			{
				throw new DomainException($"{nameof(imageBase64)} cannot be null or whitespace.");
			}

			ImageBase64 = imageBase64;
		}
	}
}
