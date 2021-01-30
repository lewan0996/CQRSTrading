using Autofac;
using CQRSTrading.Auctions.Application.Commands.CreateAuction;
using CQRSTrading.WEB.Infrastructure.Behaviors;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace CQRSTrading.WEB.Infrastructure.AutofacModules
{
	internal class MediatorModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo()
					.Assembly)
				.AsImplementedInterfaces();

			var applicationAssemblies = new[] { typeof(CreateAuctionCommand).Assembly };

			// Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
			builder.RegisterAssemblyTypes(applicationAssemblies)
				.AsClosedTypesOf(typeof(IRequestHandler<,>));

			// Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
			builder.RegisterAssemblyTypes(applicationAssemblies)
				.AsClosedTypesOf(typeof(INotificationHandler<>));

			// Register the Command's Validators (Validators based on FluentValidation library)
			builder
				.RegisterAssemblyTypes(applicationAssemblies)
				.Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
				.AsImplementedInterfaces();


			builder.Register<ServiceFactory>(context =>
			{
				var componentContext = context.Resolve<IComponentContext>();
				return t => componentContext.TryResolve(t, out var o) ? o : null;
			});

			builder.RegisterGeneric(typeof(TransactionBehaviour<,>))
				.As(typeof(IPipelineBehavior<,>));

			builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
				.As(typeof(IPipelineBehavior<,>));
		}
	}
}
