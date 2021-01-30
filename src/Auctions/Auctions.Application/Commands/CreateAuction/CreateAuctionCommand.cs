using System;
using MediatR;
using Newtonsoft.Json;

namespace CQRSTrading.Auctions.Application.Commands.CreateAuction
{
	public class CreateAuctionCommand : IRequest
	{
		[JsonIgnore]
		public Guid Id { get; set; }

		[JsonIgnore]
		public Guid UserId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public float Price { get; set; }
		public int Quantity { get; set; }
		public string ImageBase64 { get; set; }
	}
}
