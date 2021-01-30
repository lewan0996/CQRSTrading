﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CQRSTrading.WEB.Infrastructure.Behaviors
{
	public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
		private readonly IValidator<TRequest>[] _validators;

		public ValidatorBehavior(IValidator<TRequest>[] validators,
			ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
		{
			_validators = validators;
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var typeName = request.GetGenericTypeName();

			_logger.LogInformation("----- Validating command {CommandType}", typeName);

			var failures = _validators
				.Select(v => v.Validate(request))
				.SelectMany(result => result.Errors)
				.Where(error => error != null)
				.ToList();

			if (!failures.Any())
			{
				return await next();
			}

			_logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}",
				typeName, request, failures);

			throw new ValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", failures);
		}
	}
}
