using System;
using System.Threading.Tasks;
using CQRSTrading.Auctions.Application.Commands.CreateAuction;
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

		public AuctionsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> Get(Guid id) => throw new NotImplementedException();

		[HttpPost]
		public async Task<IActionResult> CreateAuction(CreateAuctionCommand command)
		{
			var id = Guid.NewGuid();
			command.Id = id;

			//command.UserId = Guid.Parse(User.GetObjectId() ?? throw new InvalidOperationException());
			command.UserId = Guid.NewGuid();

			await _mediator.Send(command);

			return AcceptedAtAction(nameof(Get), nameof(AuctionsController), new { id });
		}
	}
}
