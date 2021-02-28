using System;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;

namespace CQRSTrading.Auctions.ProjectionEvents
{
	public class AuctionCreatedProjectionEvent : IProjectionEvent
	{
		public const string EVENT_TYPE = "AuctionCreated";
		public string EventType => EVENT_TYPE;
		public Guid Id { get; }
		public Guid UserId { get; }
		public string Name { get; }
		public string Description { get; }
		public float Price { get; }
		public int Quantity { get; }
		public string ImageBase64 { get; }

		public AuctionCreatedProjectionEvent(Auction auction)
		{
			Id = auction.Id;
			UserId = auction.UserId;
			Name = auction.Name.Value;
			Description = auction.Description.Value;
			Price = auction.Price.Value;
			Quantity = auction.Quantity;
			ImageBase64 = auction.ImageBase64;
		}
	}
}
