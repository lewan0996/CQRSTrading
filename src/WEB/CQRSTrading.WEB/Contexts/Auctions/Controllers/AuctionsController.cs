using System;
using System.Threading.Tasks;
using CQRSTrading.Auctions.Application.Commands.CreateAuction;
using CQRSTrading.Auctions.ReadModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace CQRSTrading.WEB.Contexts.Auctions.Controllers
{
	//[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class AuctionsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IAuctionsReadModelRepository _auctionsReadModelRepository;

		public AuctionsController(IMediator mediator, IAuctionsReadModelRepository auctionsReadModelRepository)
		{
			_mediator = mediator;
			_auctionsReadModelRepository = auctionsReadModelRepository;
		}

		[HttpGet("{category}/{id:guid}")]
		public async Task<IActionResult> GetByCategory(string category, Guid id)
			=> Ok(await _auctionsReadModelRepository.GetAsync(id, category));

		[HttpGet("{userId:guid}/{id:guid}")]
		public async Task<IActionResult> GetByUser(Guid userId, Guid id)
			=> Ok(await _auctionsReadModelRepository.GetAsync(id, userId));

		[HttpPost]
		public async Task<IActionResult> CreateAuction(CreateAuctionCommand command)
		{
			var id = Guid.NewGuid();
			command.Id = id;

			//command.UserId = Guid.Parse(User.GetObjectId() ?? throw new InvalidOperationException());
			command.UserId = Guid.NewGuid();

			await _mediator.Send(command);

			return AcceptedAtAction(nameof(GetByUser), nameof(AuctionsController), new { command.UserId, id });
		}
	}
}
