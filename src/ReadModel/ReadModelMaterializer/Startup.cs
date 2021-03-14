using CQRSTrading.Auctions.Infrastructure.CosmosDb;
using CQRSTrading.Auctions.ReadModel;
using CQRSTrading.Shared.Infrastructure.CosmosDb;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReadModelMaterializer;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ReadModelMaterializer
{
	internal class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddSingleton<IAuctionsReadModelRepository, AuctionsCosmosDbReadModelRepository>(
				serviceProvider =>
				{
					var cosmosDbAdapter = serviceProvider.GetService<CosmosDbAdapter>();

					var auctionsCosmosDbReadModelRepository = new AuctionsCosmosDbReadModelRepository(cosmosDbAdapter);

					auctionsCosmosDbReadModelRepository.Init()
						.Wait();

					return auctionsCosmosDbReadModelRepository;
				});

			builder.Services.AddSingleton(serviceProvider =>
			{
				var config = serviceProvider.GetService<IOptions<CosmosDbConfiguration>>();

				var cosmosDbAdapter = new CosmosDbAdapter(config);

				cosmosDbAdapter.InitAsync()
					.Wait();

				return cosmosDbAdapter;
			});

			builder.Services.AddOptions<CosmosDbConfiguration>()
				.Configure<IConfiguration>((settings, configuration) =>
				{
					configuration.GetSection("CosmosDb")
						.Bind(settings);
				});

			builder.Services.AddSingleton<ReadModelMaterializeService>();
			builder.Services.AddSingleton<ProjectionEventDeserializer>();
		}
	}
}
