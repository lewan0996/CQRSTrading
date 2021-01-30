using System.Threading;
using System.Threading.Tasks;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Shared.Domain;
using MediatR;

namespace CQRSTrading.Auctions.Application.Commands.CreateAuction
{
	public class CreateAuctionCommandHandler : AsyncRequestHandler<CreateAuctionCommand>
	{
		private readonly IRepository<Auction> _auctionRepository;

		public CreateAuctionCommandHandler(IRepository<Auction> auctionRepository)
		{
			_auctionRepository = auctionRepository;
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

			await _auctionRepository.AddAsync(auction);

			await _auctionRepository.UnitOfWork.SaveEntitiesAsync();
		}
	}
}
