using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CQRSTrading.Auctions.Infrastructure.WriteModel;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Infrastructure.AzureStorageQueue;
using CQRSTrading.Shared.Infrastructure.CosmosDb;
using CQRSTrading.Shared.Infrastructure.EF;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CQRSTrading.WEB.Infrastructure.AutofacModules
{
	public class ApplicationModule : Module
	{
		private readonly IConfiguration _configuration;

		public ApplicationModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule(new MediatorModule());
			builder.RegisterModule(new AuctionsModule(_configuration));

			RegisterUnitOfWork(builder);
			RegisterAzureStorageQueue(builder);
			RegisterCosmosDb(builder);
		}

		private void RegisterUnitOfWork(ContainerBuilder builder)
		{
			var dbContextTypes = new[] { typeof(AuctionsDbContext) };

			builder.Register(c => new EFUnitOfWork(
					dbContextTypes.Select(t => c.Resolve(t) as DbContext)
						.ToArray()
				))
				.As<IUnitOfWork>()
				.InstancePerLifetimeScope();
		}

		private void RegisterAzureStorageQueue(ContainerBuilder builder)
		{
			var azureStorageQueueConfiguration = new AzureStorageQueueConfiguration();

			_configuration.GetSection("AzureStorageQueue")
				.Bind(azureStorageQueueConfiguration);

			builder.RegisterInstance(azureStorageQueueConfiguration)
				.SingleInstance();

			builder.RegisterType<AzureStorageQueueEventBus>()
				.As<IProjectionEventBus>()
				.SingleInstance()
				.OnActivated(async h => await h.Instance.InitAsync());
		}

		private void RegisterCosmosDb(ContainerBuilder builder)
		{
			builder.RegisterType<CosmosDbAdapter>()
				.AsSelf()
				.SingleInstance()
				.OnActivated(async h => await h.Instance.InitAsync());

			var serviceCollection = new ServiceCollection();

			serviceCollection.Configure<CosmosDbConfiguration>(_configuration.GetSection("CosmosDb"));

			builder.Populate(serviceCollection);
		}
	}
}
