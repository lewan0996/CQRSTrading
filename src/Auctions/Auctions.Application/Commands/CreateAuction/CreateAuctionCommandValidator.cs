using CQRSTrading.Auctions.Domain.AuctionAggregate;
using FluentValidation;

namespace CQRSTrading.Auctions.Application.Commands.CreateAuction
{
	// ReSharper disable once UnusedType.Global
	public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
	{
		public CreateAuctionCommandValidator()
		{
			RuleFor(c => c.Id)
				.NotEmpty();

			RuleFor(c => c.UserId)
				.NotEmpty();

			RuleFor(c => c.Name)
				.NotEmpty()
				.MaximumLength(AuctionName.MAX_LENGTH);

			RuleFor(c => c.Description)
				.NotEmpty()
				.MaximumLength(AuctionDescription.MAX_LENGTH);

			RuleFor(c => c.Price)
				.GreaterThanOrEqualTo(0);

			RuleFor(c => c.Quantity)
				.GreaterThan(0);

			RuleFor(c => c.ImageBase64)
				.Must(im => im.Length % 4 == 0)
				.When(c => c.ImageBase64 != null)
				.WithMessage("Base64 string length must be a multiple of 4");
		}
	}
}
