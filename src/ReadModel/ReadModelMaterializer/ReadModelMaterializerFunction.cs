using System.Threading.Tasks;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ReadModelMaterializer
{
	// ReSharper disable once UnusedType.Global
	public class ReadModelMaterializerFunction
	{
		private readonly ReadModelMaterializeService _readModelMaterializeService;

		internal ReadModelMaterializerFunction(ReadModelMaterializeService readModelMaterializeService)
		{
			_readModelMaterializeService = readModelMaterializeService;
		}

		[FunctionName("ReadModelMaterializerFunction")]
		// ReSharper disable once UnusedMember.Global
		public async Task<IActionResult> Run([QueueTrigger("projection-events", Connection = "StorageAccountConnectionString")]
			string message, ILogger log)
		{
			log.LogInformation($"C# Queue trigger function received: {message}");

			var projectionEvent = JsonConvert.DeserializeObject<IProjectionEvent>(message);

			await _readModelMaterializeService.MaterializeAsync(projectionEvent);

			return new OkObjectResult($"The projection event {message} has been materialized successfully");
		}
	}
}
