using System.Collections.Generic;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Domain.Exceptions;

namespace CQRSTrading.Auctions.Domain.AuctionAggregate
{
	public class AuctionName : ValueObject
	{
		public string Value { get; private set; }
		public const int MAX_LENGTH = 255;

		public AuctionName(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new DomainException("Auction name cannot be empty");
			}

			if (value.Length > MAX_LENGTH)
			{
				throw new DomainException($"Auction name cannot be longer than ${MAX_LENGTH} characters");
			}

			Value = value;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}
