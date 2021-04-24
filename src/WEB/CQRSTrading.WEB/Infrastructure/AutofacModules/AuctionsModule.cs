using Autofac;
using Autofac.Extensions.DependencyInjection;
using CQRSTrading.Auctions.Infrastructure.CosmosDb;
using CQRSTrading.Auctions.Infrastructure.WriteModel;
using CQRSTrading.Auctions.ReadModel;
using CQRSTrading.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Auction = CQRSTrading.Auctions.Domain.AuctionAggregate.Auction;

namespace CQRSTrading.WEB.Infrastructure.AutofacModules
{
	public class AuctionsModule : Module
	{
		private readonly IConfiguration _configuration;

		public AuctionsModule(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void Load(ContainerBuilder builder)
		{
			AddDbContext(builder);

			builder.RegisterType<AuctionsWriteModelRepository>()
				.As<IWriteModelRepository<Auction>>()
				.InstancePerLifetimeScope();

			builder.RegisterType<AuctionsCosmosDbReadModelRepository>()
				.As<IAuctionsReadModelRepository>()
				.SingleInstance()
				.OnActivated(async h => await h.Instance.InitAsync());
		}

		private void AddDbContext(ContainerBuilder builder)
		{
			var serviceCollection = new ServiceCollection();

			serviceCollection
				.AddDbContext<AuctionsDbContext>(opts =>
				{
					opts.UseSqlServer(_configuration.GetConnectionString("SqlServer"),
						sqlOptions =>
						{
							sqlOptions.MigrationsAssembly(typeof(AuctionsDbContext).Assembly.GetName()
								.Name);
							sqlOptions.MigrationsHistoryTable("__EFMigrationHistory", "Auctions");
						});
				});

			builder.Populate(serviceCollection);
		}
	}
}
