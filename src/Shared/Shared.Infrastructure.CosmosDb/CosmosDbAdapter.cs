using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace CQRSTrading.Shared.Infrastructure.CosmosDb
{
	public class CosmosDbAdapter
	{
		private const string DATABASE_NAME = "CQRSTrading";
		private readonly CosmosDbConfiguration _configuration;
		public Database Database;

		public CosmosDbAdapter(IOptions<CosmosDbConfiguration> configuration)
		{
			_configuration = configuration.Value;
		}

		public async Task InitAsync()
		{
			var cosmosClient = new CosmosClient(_configuration.Url, _configuration.Key,
				new CosmosClientOptions
				{
					SerializerOptions = new CosmosSerializationOptions
					{
						PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
					}
				});
			Database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DATABASE_NAME);
		}
	}
}
