// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CQRSTrading.Shared.Infrastructure.AzureStorageQueue
{
	public class AzureStorageQueueConfiguration
	{
		public string Uri { get; set; }
		public string QueueName { get; set; }

		// ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
		public bool DevMode { get; set; } = false;
	}
}
