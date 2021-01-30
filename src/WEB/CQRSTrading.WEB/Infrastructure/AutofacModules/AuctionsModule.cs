using Autofac;
using Autofac.Extensions.DependencyInjection;
using CQRSTrading.Auctions.Domain.AuctionAggregate;
using CQRSTrading.Auctions.Infrastructure;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

			builder.RegisterType<AuctionsRepository>()
				.As<IRepository<Auction>>()
				.InstancePerLifetimeScope();
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
