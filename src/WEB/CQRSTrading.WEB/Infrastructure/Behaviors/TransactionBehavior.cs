using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSTrading.Shared.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRSTrading.WEB.Infrastructure.Behaviors
{
	public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public TransactionBehaviour(IUnitOfWork unitOfWork,
			ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				if (_unitOfWork.HasActiveTransaction)
				{
					return await next();
				}

				TResponse response;
				using (var transaction = await _unitOfWork.BeginTransactionAsync())
				{
					var transactionId = transaction.Id;

					_logger.LogInformation(
						$"----- Begin transaction {transactionId} for {request.GetGenericTypeName()} ({request})");

					response = await next();

					_logger.LogInformation($"----- Commit transaction {transactionId} for {request.GetGenericTypeName()}");

					await _unitOfWork.CommitTransactionAsync(transaction);
				}

				return response;
			}
			catch (Exception ex)
			{
				_logger.LogError(
					$"ERROR Handling transaction for {request.GetGenericTypeName()} ({request}). {ex.Message} Stacktrace: {ex.StackTrace}");

				throw;
			}
		}
	}
}
