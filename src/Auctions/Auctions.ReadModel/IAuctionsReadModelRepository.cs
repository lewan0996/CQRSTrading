﻿using System;
using System.Threading.Tasks;

namespace CQRSTrading.Auctions.ReadModel
{
	public interface IAuctionsReadModelRepository
	{
		Task<Auction> Get(Guid id, string category);
		Task<Auction> Get(Guid id, Guid userId);
		Task Insert(Auction auction);
	}
}
