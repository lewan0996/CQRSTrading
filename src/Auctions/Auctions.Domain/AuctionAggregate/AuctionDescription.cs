using System.Collections.Generic;
using CQRSTrading.Shared.Domain;

namespace CQRSTrading.Auctions.Domain.AuctionAggregate
{
	public class AuctionDescription : ValueObject
	{
		public string Value { get; private set; }
		public const int MAX_LENGTH = 1024;

		public AuctionDescription(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new DomainException("Auction description cannot be empty");
			}

			if (value.Length > MAX_LENGTH)
			{
				throw new DomainException($"Auction description cannot be longer than ${MAX_LENGTH} characters");
			}

			Value = value;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}
