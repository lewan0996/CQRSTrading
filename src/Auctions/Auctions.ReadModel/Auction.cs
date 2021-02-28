using System;
using CQRSTrading.Shared.Application;

namespace CQRSTrading.Auctions.ReadModel
{
	public class Auction : ReadModelEntity
	{
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public float Price { get; set; }
		public int Quantity { get; set; }
		public string ImageBase64 { get; set; }
	}
}
