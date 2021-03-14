using System;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace CQRSTrading.Auctions.ReadModel.ProjectionEvents
{
	public class AuctionCreatedProjectionEvent : IProjectionEvent
	{
		public const string EVENT_TYPE = "AuctionCreated";
		public string EventType => EVENT_TYPE;
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public float Price { get; set; }
		public int Quantity { get; set; }
		public string ImageBase64 { get; set; }

		public AuctionCreatedProjectionEvent(Domain.AuctionAggregate.Auction auction)
		{
			Id = auction.Id;
			UserId = auction.UserId;
			Name = auction.Name.Value;
			Description = auction.Description.Value;
			Price = auction.Price.Value;
			Quantity = auction.Quantity;
			ImageBase64 = auction.ImageBase64;
		}

		// ReSharper disable once UnusedMember.Global
		public AuctionCreatedProjectionEvent() { } // For deserializer
	}
}
