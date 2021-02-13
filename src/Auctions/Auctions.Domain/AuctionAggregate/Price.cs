using System.Collections.Generic;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Domain.Exceptions;

namespace CQRSTrading.Auctions.Domain.AuctionAggregate
{
	public class Price : ValueObject
	{
		public float Value { get; private set; }

		public Price(float value)
		{
			if (value < 0)
			{
				throw new DomainException("Price cannot be lower than 0");
			}

			Value = value;
		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}
