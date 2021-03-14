using System;
using System.Threading.Tasks;
using CQRSTrading.Auctions.ReadModel;
using CQRSTrading.Shared.Infrastructure.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace CQRSTrading.Auctions.Infrastructure.CosmosDb
{
	public class AuctionsCosmosDbReadModelRepository : IAuctionsReadModelRepository
	{
		private const string USER_AUCTIONS_CONTAINER_NAME = "UserAuctions";
		private const string CATEGORY_AUCTIONS_CONTAINER_NAME = "Auctions";

		private readonly CosmosDbAdapter _cosmosDbAdapter;
		private Container _userAuctionsContainer;
		private Container _categoryAuctionsContainer;

		public AuctionsCosmosDbReadModelRepository(CosmosDbAdapter cosmosDbAdapter)
		{
			_cosmosDbAdapter = cosmosDbAdapter;
		}

		public async Task Init()
		{
			_userAuctionsContainer = await _cosmosDbAdapter.Database.CreateContainerIfNotExistsAsync(
				USER_AUCTIONS_CONTAINER_NAME, "/userId");

			_categoryAuctionsContainer = await _cosmosDbAdapter.Database.CreateContainerIfNotExistsAsync(
				CATEGORY_AUCTIONS_CONTAINER_NAME, "/category");
		}

		public Task Insert(Auction item) => _userAuctionsContainer.CreateItemAsync(item); // zrób trigger, który to skopiuje do kontenera partycjonowanego po kategorii

		public async Task<Auction> Get(Guid id, string category)
		{
			var response = await _categoryAuctionsContainer.ReadItemAsync<Auction>(
				id.ToString(), new PartitionKey(category));

			return response.Resource;
		}

		public async Task<Auction> Get(Guid id, Guid userId)
		{
			var response = await _userAuctionsContainer.ReadItemAsync<Auction>(
				id.ToString(), new PartitionKey(userId.ToString()));

			return response.Resource;
		}
	}
}
