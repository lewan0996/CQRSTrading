using System;
using System.Dynamic;
using CQRSTrading.Auctions.ReadModel.ProjectionEvents;
using CQRSTrading.Shared.ProjectionEvents.Abstractions;
using Newtonsoft.Json;

namespace ReadModelMaterializer
{
	public class ProjectionEventDeserializer
	{
		public IProjectionEvent Deserialize(string serializedEvent)
		{
			dynamic dynamicEvent = JsonConvert.DeserializeObject<ExpandoObject>(serializedEvent);

			return dynamicEvent.EventType switch
			{
				AuctionCreatedProjectionEvent.EVENT_TYPE =>
				JsonConvert.DeserializeObject<AuctionCreatedProjectionEvent>(serializedEvent),
				_ => throw new ArgumentOutOfRangeException(nameof(dynamicEvent.EventType),
					$"Materialization of the {dynamicEvent.EventType} is not supported")
			};
		}
	}
}
