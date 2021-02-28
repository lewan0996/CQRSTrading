using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CQRSTrading.Shared.Infrastructure.CosmosDb
{
	public class CosmosDbAdapter
	{
		private const string DATABASE_NAME = "CQRSTrading";
		private readonly CosmosDbConfiguration _configuration;
		public Database Database;

		public CosmosDbAdapter(CosmosDbConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task Init()
		{
			var cosmosClient = new CosmosClient(_configuration.Url, _configuration.Key);
			Database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DATABASE_NAME);
		}
	}
}
