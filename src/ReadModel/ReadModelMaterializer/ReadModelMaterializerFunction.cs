using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ReadModelMaterializer
{
	// ReSharper disable once UnusedType.Global
	public class ReadModelMaterializerFunction
	{
		private readonly ReadModelMaterializeService _readModelMaterializeService;
		private readonly ProjectionEventDeserializer _projectionEventDeserializer;

		public ReadModelMaterializerFunction(ReadModelMaterializeService readModelMaterializeService,
			ProjectionEventDeserializer projectionEventDeserializer)
		{
			_readModelMaterializeService = readModelMaterializeService;
			_projectionEventDeserializer = projectionEventDeserializer;
		}

		[FunctionName("ReadModelMaterializerFunction")]
		// ReSharper disable once UnusedMember.Global
		public async Task Run([QueueTrigger("projection-events", Connection = "StorageAccountConnectionString")]
			string message, ILogger log)
		{
			log.LogInformation($"C# Queue trigger function received: {message}");

			var projectionEvent = _projectionEventDeserializer.Deserialize(message);

			await _readModelMaterializeService.MaterializeAsync(projectionEvent);

			log.LogInformation($"Projection event of type {projectionEvent.EventType} has been successfully materialized");
		}
	}
}
