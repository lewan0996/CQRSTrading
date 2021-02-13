using System;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;

namespace CQRSTrading.Auctions.ProjectionEvents
{
	public class AuctionCreatedProjectionEvent : IProjectionEvent
	{
		public Guid UserId { get; }
		public string Name { get; }
		public string Description { get; }
		public float Price { get; }
		public int Quantity { get; }
		public string ImageBase64 { get; }

		public AuctionCreatedProjectionEvent(Auction auction)
		{
			UserId = auction.UserId;
			Name = auction.Name.Value;
			Description = auction.Description.Value;
			Price = auction.Price.Value;
			Quantity = auction.Quantity;
			ImageBase64 = auction.ImageBase64;
		}
	}
}
