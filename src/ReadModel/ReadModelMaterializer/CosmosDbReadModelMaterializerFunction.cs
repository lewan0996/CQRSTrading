using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ReadModelMaterializer
{
	// ReSharper disable once UnusedType.Global
	public static class CosmosDbReadModelMaterializerFunction
	{
		[FunctionName("CosmosDbReadModelMaterializerFunction")]
		// ReSharper disable once UnusedMember.Global
		public static void Run([QueueTrigger("projection-events", Connection = "StorageAccountConnectionString")]
			string myQueueItem, ILogger log)
		{
			log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
		}
	}
}
