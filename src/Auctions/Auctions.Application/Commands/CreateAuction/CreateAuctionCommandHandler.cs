using System.Threading;
using System.Threading.Tasks;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Auctions.ReadModel.ProjectionEvents;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;
using MediatR;

namespace CQRSTrading.Auctions.Application.Commands.CreateAuction
{
	// ReSharper disable once UnusedType.Global
	public class CreateAuctionCommandHandler : AsyncRequestHandler<CreateAuctionCommand>
	{
		private readonly IWriteModelRepository<Auction> _auctionWriteModelRepository;
		private readonly IProjectionEventBus _projectionEventBus;

		public CreateAuctionCommandHandler(IWriteModelRepository<Auction> auctionWriteModelRepository,
			IProjectionEventBus projectionEventBus)
		{
			_auctionWriteModelRepository = auctionWriteModelRepository;
			_projectionEventBus = projectionEventBus;
		}

		protected override async Task Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
		{
			var auction = new Auction(
				request.Id,
				request.UserId,
				new AuctionName(request.Name),
				new AuctionDescription(request.Description),
				new Price(request.Price),
				request.Quantity,
				request.ImageBase64
			);

			await _auctionWriteModelRepository.AddAsync(auction);

			await _auctionWriteModelRepository.UnitOfWork.SaveEntitiesAsync();

			await _projectionEventBus.PublishAsync(new AuctionCreatedProjectionEvent(auction));
		}
	}
}
