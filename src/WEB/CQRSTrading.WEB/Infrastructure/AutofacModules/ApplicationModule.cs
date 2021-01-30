using System.Linq;
using Autofac;
using CQRSTrading.Auctions.Infrastructure;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
	}
}
