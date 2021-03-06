﻿using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Queues;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;
using Newtonsoft.Json;

namespace CQRSTrading.Shared.Infrastructure.AzureStorageQueue
{
	public class AzureStorageQueueEventBus : IProjectionEventBus
	{
		private readonly QueueClient _queueClient;

		public AzureStorageQueueEventBus(AzureStorageQueueConfiguration configuration)
		{
			var queueClientOptions = new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 };

			_queueClient = configuration.DevMode
				? new QueueClient("UseDevelopmentStorage=true", configuration.QueueName, queueClientOptions)
				: new QueueClient(new Uri(configuration.Uri), new DefaultAzureCredential(), queueClientOptions);
		}

		public Task InitAsync() => _queueClient.CreateIfNotExistsAsync();

		public Task PublishAsync(IProjectionEvent projectionEvent)
			=> _queueClient.SendMessageAsync(JsonConvert.SerializeObject(projectionEvent));
	}
}
