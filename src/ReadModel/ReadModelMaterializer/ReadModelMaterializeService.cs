using System;
using System.Threading.Tasks;
using CQRSTrading.Auctions.ReadModel;
using CQRSTrading.Auctions.ReadModel.ProjectionEvents;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;

namespace ReadModelMaterializer
{
	public class ReadModelMaterializeService
	{
		private readonly IAuctionsReadModelRepository _auctionsReadModelRepository;

		public ReadModelMaterializeService(IAuctionsReadModelRepository auctionsReadModelRepository)
		{
			_auctionsReadModelRepository = auctionsReadModelRepository;
		}

		public Task MaterializeAsync(IProjectionEvent projectionEvent)
		{
			switch (projectionEvent.EventType)
			{
				case AuctionCreatedProjectionEvent.EVENT_TYPE:
					var auctionCreatedEvent = (AuctionCreatedProjectionEvent)projectionEvent;

					var auction = new Auction
					{
						Id = auctionCreatedEvent.Id,
						Description = auctionCreatedEvent.Description,
						ImageBase64 = auctionCreatedEvent.ImageBase64,
						Name = auctionCreatedEvent.Name,
						Price = auctionCreatedEvent.Price,
						Quantity = auctionCreatedEvent.Quantity,
						UserId = auctionCreatedEvent.UserId
					};

					return _auctionsReadModelRepository.Insert(auction);

				default:
					throw new ArgumentOutOfRangeException(nameof(projectionEvent.EventType),
						$"Materialization of the {projectionEvent.EventType} is not supported");
			}
		}
	}
}
