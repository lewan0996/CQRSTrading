﻿using System;
using CQRSTrading.Shared.Domain;

namespace CQRSTrading.Auctions.Domain.AuctionAggregate
{
	public class Auction : Entity, IAggregateRoot
	{
		public AuctionName Name { get; private set; }
		public AuctionDescription Description { get; private set; }
		public Price Price { get; private set; }
		public int Quantity { get; private set; }
		public string ImageBase64 { get; private set; }

		public Auction(AuctionName name, AuctionDescription description, Price price, int quantity, string imageBase64 = null)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			Price = price ?? throw new ArgumentNullException(nameof(price));

			if (quantity <= 0)
			{
				throw new DomainException($"{nameof(quantity)} must be greater than 0");
			}

			Quantity = quantity;

			if (imageBase64 != null)
			{
				AddImage(imageBase64);
			}
		}

		public void AddImage(string imageBase64)
		{
			if (string.IsNullOrWhiteSpace(imageBase64))
			{
				throw new DomainException($"{nameof(imageBase64)} cannot be null or whitespace.");
			}

			ImageBase64 = imageBase64;
		}
	}
}