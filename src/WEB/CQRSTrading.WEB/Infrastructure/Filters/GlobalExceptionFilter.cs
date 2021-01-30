using System;
using System.Linq;
using CQRSTrading.Shared.Domain;
using CQRSTrading.Shared.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CQRSTrading.WEB.Infrastructure.Filters
{
	internal class GlobalExceptionFilter : IExceptionFilter
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ILogger<GlobalExceptionFilter> _logger;

		public GlobalExceptionFilter(IWebHostEnvironment environment, ILogger<GlobalExceptionFilter> logger)
		{
			_environment = environment ?? throw new ArgumentNullException(nameof(environment));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void OnException(ExceptionContext context)
		{
			_logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

			switch (context.Exception)
			{
				case IDomainException _:
					OnDomainException(context);
					break;
				case ValidationException _:
					OnValidationException(context);
					break;
				case RecordNotFoundException _:
					OnRecordNotFoundException(context);
					break;
				default:
					OnDefaultException(context);
					break;
			}
		}

		private void OnDomainException(ExceptionContext context)
		{
			var problemDetails = new ValidationProblemDetails
			{
				Instance = context.HttpContext.Request.Path,
				Status = StatusCodes.Status400BadRequest,
				Detail = "Please refer to the errors property for additional details."
			};

			context.Result = new BadRequestObjectResult(problemDetails);
			context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

			problemDetails.Errors.Add("DomainValidations", new[] { context.Exception.Message });
		}

		private void OnValidationException(ExceptionContext context)
		{
			var problemDetails = new ValidationProblemDetails
			{
				Instance = context.HttpContext.Request.Path,
				Status = StatusCodes.Status400BadRequest,
				Detail = "Please refer to the errors property for additional details."
			};

			context.Result = new BadRequestObjectResult(problemDetails);
			context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

			problemDetails.Errors.Add("CommandValidations",
				((ValidationException)context.Exception).Errors
				.Select(e => e.ErrorMessage)
				.ToArray()
			);
		}

		private void OnRecordNotFoundException(ExceptionContext context)
		{
			var json = new JsonErrorResponse
				{ Messages = new[] { ((RecordNotFoundException)context.Exception).Message } };

			context.Result = new NotFoundObjectResult(json);
			context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
		}

		private void OnDefaultException(ExceptionContext context)
		{
			var json = new JsonErrorResponse { Messages = new[] { "An error occured. Try again." } };

			if (_environment.IsDevelopment())
			{
				json.DeveloperMessage = context.Exception;
			}

			context.Result = new JsonResult(json);
			context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
		}
	}

	internal class JsonErrorResponse
	{
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public string[] Messages { get; set; }

		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public object DeveloperMessage { get; set; }
	}
}